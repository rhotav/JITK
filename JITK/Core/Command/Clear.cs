using System;

namespace JITK.Core.Command
{
    class Clear : Command
    {
        override public string HelpText { get { return "Clear Console"; } }
        override public string Name { get { return "clear"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            Console.Clear();
        }
    }
}
