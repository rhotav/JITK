using System;

namespace JITK.Core.Command
{
    class DefBreakPoint : Command
    {
        override public string HelpText { get { return "Define breakpoint"; } }
        override public string Name { get { return "b"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.Single; } }
        override public void Action()
        {
            if (CommandEngine.arg[0].Length == 8)
            {
                int id = Context.breakPoints.Count + 1;
                Context.breakPoints.Add(id, CommandEngine.arg[0]);
                Style.WriteFormatted($"[^] Breakpoint defined. ID: {id}\n", ConsoleColor.White);
            }
            else
            {
                Style.WriteFormatted("[!] Upss! Wrong format! Ex: \"b 06000002\"\n", ConsoleColor.Red);
                return;
            }

        }

        
    }
}
