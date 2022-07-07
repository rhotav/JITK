using System;
using System.Collections.Generic;

namespace JITK.Core.Command
{
    class Help : Command
    {
        override public string HelpText { get { return "Help"; } }
        override public string Name { get { return "help"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            string result = "";
            foreach (KeyValuePair<string, Command> com in CommandEngine.commands)
            {
                result += com.Key + " -- " + com.Value.HelpText + " \n";
            }
            Style.WriteFormatted(result);
        }
    }
}
