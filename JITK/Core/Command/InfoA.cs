using System;

namespace JITK.Core.Command
{
    class InfoA : Command
    {
        override public string HelpText { get { return "Get assembly info of target module."; } }
        override public string Name { get { return "infoa";  } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            string result = "";
            result += " Assembly-Info: " + Context.module.Assembly.FullName;
            result += "\n EntryPoint: " + Context.module.EntryPoint.MDToken.ToString();
            result += "\n Machine: " + Context.module.Machine.ToString();
            result += "\n Characteristics: " + Context.module.Characteristics.ToString();
            result += "\n HasResources: " + Context.module.HasResources.ToString();
            result += "\n HasExportedTypes: " + Context.module.HasExportedTypes.ToString();

            Style.WriteFormatted(result + "\n", ConsoleColor.DarkGray);
        }



    }
}
