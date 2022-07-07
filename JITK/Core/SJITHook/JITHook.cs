using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JITK.Core.SJITHook
{
    public unsafe class JITHook<T> where T : VTableAddrProvider
    {
        private readonly T _addrProvider;
        public Data.CompileMethodDel OriginalCompileMethod { get; private set; }

        public JITHook()
        {
            _addrProvider = Activator.CreateInstance<T>();
        }

        public bool Hook(Data.CompileMethodDel hookedCompileMethod)
        {
            IntPtr pVTable = _addrProvider.VTableAddr;
            IntPtr pCompileMethod = Marshal.ReadIntPtr(pVTable);
            uint old;

            if (
                !Data.VirtualProtect(pCompileMethod, (uint)IntPtr.Size,
                    Data.Protection.PAGE_EXECUTE_READWRITE, out old))
                return false;

            OriginalCompileMethod =
                (Data.CompileMethodDel)
                    Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(pCompileMethod), typeof(Data.CompileMethodDel));

            // We don't want any infinite loops :-)
            RuntimeHelpers.PrepareDelegate(hookedCompileMethod);
            RuntimeHelpers.PrepareDelegate(OriginalCompileMethod);
            RuntimeHelpers.PrepareMethod(GetType().GetMethod("UnHook").MethodHandle, new[] { typeof(T).TypeHandle });

            Marshal.WriteIntPtr(pCompileMethod, Marshal.GetFunctionPointerForDelegate(hookedCompileMethod));

            return Data.VirtualProtect(pCompileMethod, (uint)IntPtr.Size,
                (Data.Protection)old, out old);
        }

        public bool UnHook()
        {
            IntPtr pVTable = _addrProvider.VTableAddr;
            IntPtr pCompileMethod = Marshal.ReadIntPtr(pVTable);
            uint old;

            if (
                !Data.VirtualProtect(pCompileMethod, (uint)IntPtr.Size,
                    Data.Protection.PAGE_EXECUTE_READWRITE, out old))
                return false;

            Marshal.WriteIntPtr(pCompileMethod, Marshal.GetFunctionPointerForDelegate(OriginalCompileMethod));

            return Data.VirtualProtect(pCompileMethod, (uint)IntPtr.Size,
                (Data.Protection)old, out old);
        }

        public void PrepareMethods(Assembly asm)
        {
            Module[] mods = asm.GetLoadedModules();
            foreach (Module mod in mods)
            {
                Type[] classes = mod.GetTypes();
                foreach (Type c in classes)
                {
                    MethodInfo[] methods = c.GetMethods();
                    foreach (MethodInfo method in methods)
                    {
                        try
                        {
                            RuntimeHelpers.PrepareMethod(method.MethodHandle);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("[!] Failed to prepare method 0x{0:X}", method.MetadataToken);
                        }
                    }
                }
            }
        }
    }
}
