using System;

namespace JITK.Core.Command
{
    class FileAssign : Command
    {
        override public string HelpText { get { return "Assign File"; } }
        override public string Name { get { return "f"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.Single; } }
        override public void Action()
        {
            if (Context.IsNet(CommandEngine.arg[0]))
            {
                Context.filePath = CommandEngine.arg[0];
                Style.WriteFormatted($"[^] {Context.filePath} assigned to the file path.\n");
            }
            else
            {
                Style.WriteFormatted("[!] Upss! This file is not .NET! Please specify another assembly!", ConsoleColor.Red);
            }
        }
    }
}
