using System;
using System.Collections.Generic;

namespace JITK.Core.Command
{
    static class CommandEngine
    {

        /*
         * Command Syntax :
         *  OPCODE OPERAND|NONE
         *  
         *  ~ NONE Operand Example:
         *  INFOA|infoa  -> Info Assembly Command
         *  QUIT|quit -> Exit JITK.
         *  lm  -> List All Module
         *  
         *  ~ OPCODE with Example:
         *  f %s  -> Assign File Path
         *  ....
         */


        public static Dictionary<string, Command> commands = new Dictionary<string, Command>();
        public static int CommandCount;
        public static List<string> arg = new List<string>();

        public static void LoadCommands()
        {
            Command[] commandTypes = {
                new Quit(),
                new InfoA(),
                new Clear(),
                new About(),
                new Help(),
                new InfoF(),
                new FileClear(),
                new FileAssign(),
                new FileArgument(),
                new Continue(),
                new StepFuncG(),
                new DefBreakPoint(),
                new ListBreakpoint(),
                new Disas(),
                new DisasH()
            };

            for (CommandCount = 0; CommandCount < commandTypes.Length; CommandCount++)
            {
                commands.Add(commandTypes[CommandCount].Name, commandTypes[CommandCount]);
            }
            
            Style.WriteFormatted($"[^] Loaded {CommandCount} command!\n");
        }

        public static bool MainRoutine()
        {
            while (true)
            {
                string input = Style.WriteState(">");
                ExecuteCommand(input);
            }

        }

        public static unsafe bool ExecuteCommand(string commandText)
        {
           
            if (commandText == null || commandText.Trim().Length == 0) 
            {
                Style.WriteFormatted("[!] Type a command!", ConsoleColor.Red);
                return false;
            }

            string[] splittedText = commandText.Trim().Split(' ');
            if (commands.ContainsKey(splittedText[0]))
            {
                var selectedCommand = commands[splittedText[0]];
                switch (selectedCommand.ArgSize)
                {
                    case ArgumentSize.None:
                        {
                            if (splittedText.Length > 1 || splittedText.Length < 1)
                            {
                                Style.WriteFormatted("[!] Wrong argument size.", ConsoleColor.Red);
                                return false;
                            }


                            if (Context._jitHook != null)
                            {
                                Context._jitHook.UnHook();
                                selectedCommand.Action();
                                Context._jitHook.Hook(Context.HookedCompileMethod);
                            }
                            else
                            {
                                selectedCommand.Action();
                            }
                            break;
                        }
                    case ArgumentSize.Single:
                        {
                            if (splittedText.Length > 2 || splittedText.Length < 2)
                            {
                                Style.WriteFormatted("[!] Wrong argument size.\n", ConsoleColor.Red);
                                return false;
                            }

                            arg.Add(splittedText[1]);

                            if (Context._jitHook != null)
                            {
                                Context._jitHook.UnHook();
                                selectedCommand.Action();
                                Context._jitHook.Hook(Context.HookedCompileMethod);
                            }
                            else
                            {
                                selectedCommand.Action();
                            }

                            arg.Clear();
                            break;
                        }
                    case ArgumentSize.Opt:
                        {
                            for (int i = 1; i < splittedText.Length; i++)
                            {
                                arg.Add(splittedText[i]);
                            }

                            if (Context._jitHook != null)
                            {
                                Context._jitHook.UnHook();
                                selectedCommand.Action();
                                Context._jitHook.Hook(Context.HookedCompileMethod);
                            }
                            else
                            {
                                selectedCommand.Action();
                            }

                            arg.Clear();
                            break;
                        }
                    case ArgumentSize.Poly:
                        {
                            if (splittedText.Length < 1)
                            {
                                Style.WriteFormatted("[!] Wrong argument size.\n", ConsoleColor.Red);
                                return false;
                            }

                            for (int i = 1; i < splittedText.Length; i++)
                            {
                                arg.Add(splittedText[i]);
                            }

                            if (Context._jitHook != null)
                            {
                                Context._jitHook.UnHook();
                                selectedCommand.Action();
                                Context._jitHook.Hook(Context.HookedCompileMethod);
                            }
                            else
                            {
                                selectedCommand.Action();
                            }

                            arg.Clear();
                            break;
                        }
                }
            }
            else
            {
                    Style.WriteFormatted("[!] Type a valid command!\n", ConsoleColor.Red);
            }
            return true;
        }


    }
}
