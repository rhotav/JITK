using System;

namespace JITK.Core.Command
{
    class FileArgument : Command
    {
        override public string HelpText { get { return "Assign File Argument/s"; } }
        override public string Name { get { return "fg"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.Poly; } }
        override public void Action()
        {
            foreach (string arguments in CommandEngine.arg)
            {
                Context.arguments += $"{arguments} "; 
            }
            Style.WriteFormatted($"[^] Arguments assigned.\n");
        }
    }
}
