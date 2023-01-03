using System;
using System.Drawing;
using System.IO;
using JITK.Core;
using Sharprompt;
using System.Text;
using JITK.Core.Command;

namespace JITK
{
    class Program
    {
        const string banner = @"
       _ _____ _______ _  __
      | |_   _|__   __| |/ /
      | | | |    | |  | ' / 
  _   | | | |    | |  |  <  
 | |__| |_| |_   | |  | . \ 
  \____/|_____|  |_|  |_|\_\
                            
        by rhotav";


        static void Main(string[] args)
        {
            Style.WriteFormatted(banner + "\n\n", ConsoleColor.DarkRed);
            Style.LoadKeywords();
            CommandEngine.LoadCommands();

            string path = "";
            try
            {
                    if (Context.IsNet(args[0]))
                    {
                        Context.filePath = args[0];
                    }
                    else
                    {
                        Style.WriteFormatted("[!] Upss! This file is not .NET! Please specify another assembly!", ConsoleColor.Red);
                        while (!Context.IsNet(path))
                        {
                            path = Style.WriteError("File Path");
                        }

                        Context.filePath = path;
                    }
            }
            catch
            {
                while (!Context.IsNet(path))
                {
                    path = Style.WriteError("File Path");
                }

                Context.filePath = path;
            }

            CommandEngine.MainRoutine();



            Console.ReadKey();
        }
    }
}
