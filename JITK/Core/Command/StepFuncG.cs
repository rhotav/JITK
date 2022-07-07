using JITK.Core.SJITHook;
using System;
using System.Linq;

namespace JITK.Core.Command
{
    class StepFuncG : Command
    {
        override public string HelpText { get { return "Step next function."; } }
        override public string Name { get { return "g"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }

        override public unsafe void Action()
        {
            if (Context.hookState == false)
            {
                Style.WriteFormatted("[!] Hook mode is not activated!\n", ConsoleColor.Red);
                return;
            }
            
            if (Context.hookState == true)
            {
                Context.continueOtherFunc = false;
                Context.stepOtherFunc = true;
            }
        }

    }
}
