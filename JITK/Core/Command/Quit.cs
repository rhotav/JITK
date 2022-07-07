using System;

namespace JITK.Core.Command
{
    class Quit : Command
    {
        override public string HelpText { get { return "Suspend JIT Killer"; } }
        override public string Name { get { return "quit"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            Environment.Exit(0);
        }
    }
}
