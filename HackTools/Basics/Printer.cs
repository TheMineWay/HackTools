using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class Printer
    {
        public static void Print(string text, char startDetector = '&', char endDetector = ';', bool newLine = true)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            string[] parts = text.Split(startDetector);
            if (newLine) Console.Write("\n");
            foreach(string part in parts)
            {
                string[] line = part.Split(endDetector);
                if (line.Length != 2) continue;
                Console.ForegroundColor = GetColor(line[0]);
                Console.Write(line[1]);
            }
            Console.ForegroundColor = startColor;
            if (newLine) Console.Write("\n");
        }
        public static string Fill(string val, int times)
        {
            string text = "";
            for (int i = 0; i < times; i++) text += val;
            return text;
        }

        static ConsoleColor GetColor(string color)
        {
            switch(color.ToLower())
            {
                case "black": return ConsoleColor.Black;
                case "dblue": return ConsoleColor.DarkBlue;
                case "dgreen": return ConsoleColor.DarkGreen;
                case "dcyan": return ConsoleColor.DarkCyan;
                case "dred": return ConsoleColor.DarkRed;
                case "dmagenta": return ConsoleColor.DarkMagenta;
                case "dyellow": return ConsoleColor.DarkYellow;
                case "gray": return ConsoleColor.Gray;
                case "dgray": return ConsoleColor.DarkGray;
                case "blue": return ConsoleColor.Blue;
                case "green": return ConsoleColor.Green;
                case "cyan": return ConsoleColor.Cyan;
                case "red": return ConsoleColor.Red;
                case "magenta": return ConsoleColor.Magenta;
                case "yellow": return ConsoleColor.Yellow;
                case "white": return ConsoleColor.White;
                case "rand":
                    Random random = new Random();
                    return (ConsoleColor)random.Next(0,16);
            }
            return ConsoleColor.White;
        }
    }
}