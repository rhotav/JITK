namespace JITK.Core.Command
{
    class FileClear : Command
    {
        override public string HelpText { get { return "Path Clear"; } }
        override public string Name { get { return "fc"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            Context.filePath = "";
            Style.WriteFormatted("[^] filePath Cleaned.\n");
        }
    }
}
