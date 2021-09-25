using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class InputField
    {
        public static string Text(bool allowEmpty = false, string title = "Username: ")
        {
            do
            {
                Printer.Print($"&cyan;\t{title}", newLine: false);
                string text = Console.ReadLine();
                if(allowEmpty || text != "") return text;
            } while (true);
        }
        public static string Password(bool allowEmpty = false, string title ="Password: ")
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            string text = Text(allowEmpty, title);
            Console.ForegroundColor = c;
            return text;
        }
    }
}