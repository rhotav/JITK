using System;
using dnlib.DotNet;

namespace JITK.Core.Command
{
    class InfoF : Command
    {
        override public string HelpText { get { return "Get functions from loaded module."; } }
        override public string Name { get { return "infof"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.None; } }
        override public void Action()
        {
            string result = "";
            foreach (TypeDef type in Context.module.Types)
            {
                if (!type.HasMethods) continue;
                foreach (MethodDef method in type.Methods)
                {
                    result += $"Name: {type.Name}.{method.Name} (0x{method.MDToken})\n ";
                }
            }

            Style.WriteFormatted(result + "\n", ConsoleColor.DarkGray);
        }



    }
}
