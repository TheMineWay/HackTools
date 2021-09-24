using System;
using System.Collections.Generic;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Menu<int>.MenuItem> items = new List<Menu<int>.MenuItem>();
            for(int i = 0; i < 15; i++)
            {
                items.Add(new Menu<int>.MenuItem($"Opt {i}",i));
            }
            Menu<int> menu = new Menu<int>
            (
                title: "Test Menu",
                items: items.ToArray()
            );
            menu.DisplayMenu();
        }
    }
}