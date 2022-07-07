using System;
using dnlib.DotNet;
using System.IO;
using System.Reflection;
using JITK.Core.SJITHook;
using JITK.Core.Command;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Dis2Msil;

namespace JITK.Core
{
    class Context
    {
        public static string filePath = "";
        public static string arguments = "";
        public static ModuleDefMD module = null;
        public static Assembly assembly = null;

        public static string currentToken = "00000000";

        public static bool hookState = false;
        public static bool continueOtherFunc = false;
        public static bool stepOtherFunc = false;
        public static JITHook64<ClrjitAddrProvider> _jitHook;

        public static Dictionary<int, string> breakPoints = new Dictionary<int, string>();
        public static string currentMethodBody = "";

        public static bool IsNet(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    module = ModuleDefMD.Load(path);
                    assembly = Assembly.LoadFile(path);

                    Style.WriteFormatted($"[*] {module.Name} is loaded!\n");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public static unsafe int HookedCompileMethod(IntPtr thisPtr, [In] IntPtr corJitInfo,
            [In] Data.CorMethodInfo64* methodInfo, Data.CorJitFlag flags,
            [Out] IntPtr nativeEntry, [Out] IntPtr nativeSizeOfCode)
        {
            int token = (0x06000000 + *(ushort*)methodInfo->methodHandle);
            var bodyBuffer = new byte[methodInfo->ilCodeSize];
            Marshal.Copy(methodInfo->ilCode, bodyBuffer, 0, bodyBuffer.Length);
            currentMethodBody = BitConverter.ToString(bodyBuffer);
           
            Console.WriteLine("==============================================");
            Style.WriteFormatted($"[>] Intercepted {token:x8}\n", ConsoleColor.Blue);
            Style.WriteFormatted($"[>] Body Size: {methodInfo->ilCodeSize}\n", ConsoleColor.Blue);
            currentToken = token.ToString("x8");
            while (true)
            {
                if (breakPoints.ContainsValue(currentToken.Replace("0x", ""))) 
                {
                    Style.WriteFormatted($"[^] Hit breakpoint!\n", ConsoleColor.DarkCyan);
                    continueOtherFunc = false;
                    stepOtherFunc = false;
                }
            if (continueOtherFunc == true)
                {
                    break;
                }
                string returnable = Style.WriteState(">");
                if (CommandEngine.ExecuteCommand(returnable))
                {

                    if (stepOtherFunc == true)
                    {
                        stepOtherFunc = false;
                        break;
                    }
                    if (continueOtherFunc == true)
                    {
                        return _jitHook.OriginalCompileMethod(thisPtr, corJitInfo, methodInfo, flags, nativeEntry, nativeSizeOfCode);
                    }

                }
            }

            return _jitHook.OriginalCompileMethod(thisPtr, corJitInfo, methodInfo, flags, nativeEntry, nativeSizeOfCode);
        }

    }
}
