using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HackTools
{
    class TextCircularQueue
    {
        string[] queue;
        int index = 0;

        public TextCircularQueue(string[] queue) => this.queue = queue;

        public string Next(int by = 1)
        {
            index += by;
            index = index < 0 ? queue.Length - 1 : (index >= queue.Length ? 0 : index);
            return queue[index];
        }
    }

    class UIComponents
    {
        public static void Error(string title, bool clear = true)
        {
            if(clear) Console.Clear();
            Printer.Print($"$dred; [!]: &red;{title}");
            PressAnyKey();
            if (clear) Console.Clear();
        }

        public static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static bool GetYesNo(bool clearOnFail = false, bool printOptions = true)
        {
            do
            {
                if (printOptions) Console.Write("(Y/N) ");
                ConsoleKey res = Console.ReadKey().Key;
                if (res == ConsoleKey.Y) return true;
                if (res == ConsoleKey.N) return false;
                if (clearOnFail) Console.Clear();
            } while (true);
        }

        public static void CenterAndPrint(string[] lines)
        {
            foreach (string line in lines.Select((e) => Center(e)).ToArray()) Console.WriteLine(line);
        }

        public static string Center(string line)
        {
            int sides = Console.WindowWidth - line.Length;
            if (sides <= 1) return line;

            sides /= 2;

            return Printer.Fill(" ", sides) + line + Printer.Fill(" ", ((Console.WindowWidth - line.Length) % 2 == 0 ? sides : sides - 1));
        }
    }
}