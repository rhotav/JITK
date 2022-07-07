using Sharprompt;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace JITK.Core
{

    class Style
    {
        public static Symbol error = new Symbol("[!] >", "[!]");
        public static Symbol question = new Symbol("[?] >", "[?]");
        public static Symbol done = new Symbol("[*]", "[^]");
        public static Symbol selector = new Symbol(">>", "");
        public static Symbol state = new Symbol($"[{Context.currentToken}]", "");

        public static Dictionary<string, ConsoleColor> keywords = new Dictionary<string, ConsoleColor>();
        public static void LoadKeywords()
        {
            keywords.Add("[*]", ConsoleColor.DarkGreen);
            keywords.Add("[^]", ConsoleColor.DarkGreen);
            keywords.Add("[?]", ConsoleColor.DarkBlue);
            keywords.Add("[!]", ConsoleColor.DarkRed) ;
            keywords.Add("[>]", ConsoleColor.DarkYellow);

            keywords.Add("Assembly-Info:", ConsoleColor.White);
            keywords.Add("EntryPoint:", ConsoleColor.White);
            keywords.Add("Machine:", ConsoleColor.White);
            keywords.Add("Characteristics:", ConsoleColor.White);
            keywords.Add("HasResources:", ConsoleColor.White);
            keywords.Add("HasExportedTypes:", ConsoleColor.White);
            keywords.Add("JITK", ConsoleColor.DarkRed);
            keywords.Add("polynomen", ConsoleColor.DarkGray);
            keywords.Add("Name:", ConsoleColor.White);
            keywords.Add("Token:", ConsoleColor.DarkMagenta);
            keywords.Add("ID:", ConsoleColor.DarkMagenta);


        }

        public static void WriteFormatted(string text, ConsoleColor defaultColor = ConsoleColor.Gray)
        {
            string[] strArray = text.Split(' ');
            for (int i = 0; i < strArray.Length; i++)
            {
                if (keywords.ContainsKey(strArray[i]))
                {
                    Console.ForegroundColor = keywords[strArray[i]];
                    Console.Write(strArray[i] + " ");
                }
                else
                {
                    Console.ForegroundColor = defaultColor;
                    Console.Write(strArray[i] + " ");
                }
            }
            Console.ResetColor();
        }

        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public static string WriteState(string Text)
        {
            state = new Symbol($"[{Context.currentToken}]", $"[{Context.currentToken}]");
            Prompt.Symbols.Prompt = state;
            Prompt.Symbols.Done = done;
            Prompt.ColorSchema.DoneSymbolColor = ConsoleColor.Green;
            Prompt.ColorSchema.PromptSymbolColor = ConsoleColor.DarkYellow;
            Prompt.ColorSchema.Answer = ConsoleColor.White;

            string returnable = Prompt.Input<string>(Text);
            return returnable;
        }


        public static string WriteError(string Text) {
            
            Prompt.Symbols.Prompt = error;
            Prompt.Symbols.Done = done;
            Prompt.ColorSchema.DoneSymbolColor = ConsoleColor.Red;
            Prompt.ColorSchema.PromptSymbolColor = ConsoleColor.DarkRed;
            Prompt.ColorSchema.Answer = ConsoleColor.Red;

            string returnable = Prompt.Input<string>(Text);
            return returnable;
        }

    }
}
