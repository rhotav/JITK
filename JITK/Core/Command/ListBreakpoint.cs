using System;
using JITK.Core;
using System.Collections.Generic;

namespace JITK.Core.Command
{
    class ListBreakpoint : Command
    {
        override public string HelpText { get { return "List All Breakpoint"; } }
        override public string Name { get { return "bl"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            foreach (KeyValuePair<int, string> bps in Context.breakPoints)
            {
                Style.WriteFormatted($"ID: {bps.Key} -- Token: {bps.Value}\n", ConsoleColor.DarkGray);
            }
        }
    }
}
