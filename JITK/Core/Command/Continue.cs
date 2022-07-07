using JITK.Core.SJITHook;
using System;
using System.Linq;
using Dis2Msil;

namespace JITK.Core.Command
{
    class Continue : Command
    {
        override public string HelpText { get { return "Continue or Start Hook Process."; } }
        override public string Name { get { return "c"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }

        /*
         * What is prepareMethods? by *Washi* from "Official RTN Discord Server"
         * Suppose your callback is in assembly A1, and it is using method M that is defined in an external assembly A2.
            Then A2.M still has to be JIT'ed
            Even if all methods from A1 are already JIT'ed
         * 
         * 
         */

        override public unsafe void Action()
        {
            if (Context.hookState == false && Context.filePath.Trim() == "")
            {
                Style.WriteFormatted("[!] File path is not assigned!\n", ConsoleColor.Red);
                return;
            }
            if (Context.hookState == false)
            {
                Context._jitHook = new JITHook64<ClrjitAddrProvider>();
                Style.WriteFormatted("[^] Hook process is starting...\n");
                Style.WriteFormatted("[^] Methods are preparing for pre-jit\n");
                Context._jitHook.PrepareMethods(Context.assembly);
                Context._jitHook.PrepareMethods(System.Reflection.Assembly.GetExecutingAssembly());

                object[] parameters = null;
                if (Context.assembly.EntryPoint.GetParameters().Length != 0)
                {
                    parameters = new object[]
                    {
                        Context.arguments.Select(i => i.ToString()).ToArray()
                    };
                }
                Context.hookState = true;
                Style.WriteFormatted("[?] Hook mode activated.\n", ConsoleColor.Blue);
                Style.WriteFormatted("[?] Invoking...\n", ConsoleColor.Blue);

                if (Context._jitHook.Hook(Context.HookedCompileMethod))
                {
                    
                    Context.assembly.EntryPoint.Invoke(null, parameters);
                }
                return;
            }
            if (Context.hookState == true)
            {
                Context.continueOtherFunc = true;
                return;
            }
        }

    }
}
