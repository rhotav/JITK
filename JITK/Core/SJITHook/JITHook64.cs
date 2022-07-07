using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JITK.Core.SJITHook
{
    public unsafe class JITHook64<T> where T : VTableAddrProvider
    {
        private readonly T _addrProvider;
        public Data.CompileMethodDel64 OriginalCompileMethod { get; private set; }

        public JITHook64()
        {
            _addrProvider = Activator.CreateInstance<T>();
        }

        public bool Hook(Data.CompileMethodDel64 hookedCompileMethod)
        {
            IntPtr pVTable = _addrProvider.VTableAddr;
            IntPtr pCompileMethod = Marshal.ReadIntPtr(pVTable);
            uint old;

            if (
                !Data.VirtualProtect(pCompileMethod, (uint)IntPtr.Size,
                    Data.Protection.PAGE_EXECUTE_READWRITE, out old))
                return false;

            OriginalCompileMethod =
                (Data.CompileMethodDel64)
                    Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(pCompileMethod), typeof(Data.CompileMethodDel64));

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
                        catch (Exception ex)
                        {
                            if (asm == Assembly.GetExecutingAssembly())
                            {
                                continue;
                            }
                            Console.WriteLine(ex.Message + " 0x{0:X}", method.MetadataToken);
                        }
                    }
                }
            }
        }
    }
}
