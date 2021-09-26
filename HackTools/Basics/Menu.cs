using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class Menu<T>
    {
        public Menu(MenuItem[] items, string title)
        {
            this.items = items;
            this.title = title;
            cursor = "$";
        }

        string cursor;
        int perPage = 10; // Lines per page
        public string title;
        private MenuItem[] items;

        // Behaviour

        public T DisplayMenu()
        {
            int line = 0;
            do {
                perPage = Console.WindowHeight - 5;
                if (perPage <= 0) perPage = 1;
                Display();

                ConsoleKey key = Console.ReadKey().Key;

                switch(key)
                {
                    case ConsoleKey.UpArrow: line--; break;
                    case ConsoleKey.DownArrow:
                        if (line + 1 < items.Length) line++;
                        break;
                    case ConsoleKey.LeftArrow: line -= perPage; break;
                    case ConsoleKey.RightArrow:
                        if (line + perPage < items.Length - 1) line += perPage;
                        else line = items.Length - 1;
                        break; // Needs verification
                    case ConsoleKey.Escape: return default(T);
                    case ConsoleKey.Enter: return items[line].value;
                }
                if (line < 0) line = 0; // Verification

            } while (true);

            void Display()
            {
                Console.Clear();
                Printer.Print($"&white;{Printer.Fill(" ", cursor.Length + 1)}{title} - &red;Page {(line / perPage) + 1}\n");
                MenuItem[] currentItems = GetItems();
                int c = perPage * (int)(line / perPage);
                foreach(MenuItem item in currentItems)
                {
                    if (c == line) Printer.Print($"&red;{cursor} ", newLine: false);
                    else Console.Write(Printer.Fill(" ", cursor.Length + 1));
                    Console.WriteLine(item.name);
                    c++; // Not the programming language, it is a counter
                }
            }

            MenuItem[] GetItems() {
                List<MenuItem> _items = new List<MenuItem>();
                for (int i = perPage * (int)(line / perPage); i < items.Length && i <= (perPage * (int)(line / perPage)) + perPage - 1; i++) _items.Add(items[i]);
                return _items.ToArray();
            }
    }

        public class MenuItem
        {
            public MenuItem(string name, T value)
            {
                _name = name;
                _value = value;
            }

            private string _name;
            public string name => _name;
            private T _value;
            public T value => _value;
        }
        private class Selector
        {
            public int page = 0;
            public int line = 0;

            public int GetPage(int maxPage) => page > maxPage ? maxPage : (page < 0 ? 0 : page);
        }
    }
    public abstract class ScreenOption
    {
        public abstract void Open();
    }
}