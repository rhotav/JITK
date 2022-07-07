using System;

namespace JITK.Core.Command
{
    class DisasH : Command
    {
        override public string HelpText { get { return "Print method body as hex"; } }
        override public string Name { get { return "disash"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            Console.WriteLine(Context.currentMethodBody);

        }


    }
}
