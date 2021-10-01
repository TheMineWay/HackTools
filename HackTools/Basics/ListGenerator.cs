﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class ListGenerator<T, K> where T : ListItem<K>, new()
    {
        List<T> items = new List<T>();
        public T[] GetItems() => items.ToArray();
        public void AddRange(T[] range) => items.AddRange(range);
        public void Add(T val) => items.Add(val);
        public void Set(T[] values) => items = new List<T>(values);
        public void Clear() => items.Clear();

        // Displays the modification UI
        public void Modify()
        {
            int index = 0;
            int perPage;
            int page;
            do
            {
                Console.Clear();
                perPage = Console.WindowHeight - 12 >= 3 ? Console.WindowHeight - 12 : 3;
                page = (index / perPage);
                Printer.Print($"&white; Items count: &cyan;{items.Count}");
                Console.WriteLine(Printer.Fill("-", Console.BufferWidth));
                Display();

                Console.WriteLine();
                Console.WriteLine(Printer.Fill("-", Console.BufferWidth));

                // Actions
                Printer.Print("&cyan; A: &white;Add item (top)\t&cyan;Esc: &white;Save and exit\n &cyan;Z: &white;Add item (bottom)\t&cyan;D: &white;Delete");

                ConsoleKey key = Console.ReadKey().Key;
                T newItem;
                switch (key)
                {
                    case ConsoleKey.UpArrow: index--; break;
                    case ConsoleKey.DownArrow: index++; break;
                    case ConsoleKey.A:
                        newItem = new T();
                        if (newItem.AskForValue()) items.Insert(index,newItem);
                        break;
                    case ConsoleKey.Z:
                        newItem = new T();
                        if (newItem.AskForValue())
                        {
                            if (items.Count == 0)
                            {
                                items.Add(newItem);
                                break;
                            }
                            items.Insert(index >= items.Count ? items.Count - 1 : index + 1, newItem);
                        }
                        break;
                    case ConsoleKey.D:
                        if (items.Count == 0) break;
                        Printer.Print($"&red;\n Do you want to delete this item? (&white;{items[index].GetName()}&red;) ", newLine: false);
                        if (UIComponents.GetYesNo()) items.RemoveAt(index);
                        break;
                    case ConsoleKey.Escape:
                        items.Clear();
                        return;
                }

                if (index < 0) index = items.Count - 1;
                if (index >= items.Count) index = 0;
            } while (true);

            void Display()
            {
                if(items.Count <= 0)
                {
                    Printer.Print("&cyan;-- The list is empty --");
                    return;
                }
                for(int i = perPage * page; i < (perPage + (perPage * page)) && i < items.Count; i++)
                {
                    if (i != perPage * page) Console.WriteLine();
                    Printer.Print($"{(index == i ? "&red;" : "&white;")} {items[i].GetName()}", newLine: false);
                }
            }
        }
    }

    abstract class ListItem<T>
    {
        protected string name = "";
        protected T value;
        public string GetName() => name;
        public void SetName(string name) => this.name = name;
        public T GetValue() => value;
        public void SetValue(T value) => this.value = value;
        public abstract bool AskForValue();
    }
}