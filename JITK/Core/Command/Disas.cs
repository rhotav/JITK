using System;
using Dis2Msil;
using dnlib.DotNet;

namespace JITK.Core.Command
{
    class Disas : Command
    {
        override public string HelpText { get { return "Disassemble Method"; } }
        override public string Name { get { return "disas"; } }
        override public ArgumentSize ArgSize { get { return ArgumentSize.Opt; } }
        override public unsafe void Action()
        {
            if (CommandEngine.arg.Count > 1)
            {
                Style.WriteFormatted("[!] Upss! Wrong format! Ex: \"disas 06000002\"\n", ConsoleColor.Red);
                return;
            }
            if (CommandEngine.arg.Count > 0) //Static Disassemble with DNLIB thanks to 0xd4d a.k.a. wtfsck
            {
                foreach (TypeDef type in Context.module.Types)
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        if (method.MDToken.ToString() == CommandEngine.arg[0])
                        {
                            string result = "";
                            for (int i = 0; i < method.Body.Instructions.Count; i++)
                            {
                                result += $"{method.Body.Instructions[i].OpCode}  {method.Body.Instructions[i].Operand}\n";
                            }
                            Console.Write(result);
                        }
                    }
                }
            }
            if (CommandEngine.arg.Count == 0) //Dynamic disassemble with Dis2MSIL by rhotav and CodeProject
            {
                try
                {
                    MethodBodyReader mr = new MethodBodyReader(Context.assembly.EntryPoint.Module, Context.currentMethodBody);
                    Console.Write(mr.GetBodyCode());
                }
                catch { }
            }
        }
    }
}
