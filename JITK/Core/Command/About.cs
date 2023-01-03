using System;

namespace JITK.Core.Command
{
    class About : Command
    {
        override public string HelpText { get { return "About JITK"; } }
        override public string Name { get { return "about"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            Style.WriteFormatted("JITK - JIT Killer is hooker for clrjit. Created by rhotav \n github.com/rhotav \n", ConsoleColor.Blue);
        }
    }
}
