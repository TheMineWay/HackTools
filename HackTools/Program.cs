﻿using System;
using System.Collections.Generic;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            ListGenerator<IPAndNamesList, string> listGenerator = new ListGenerator<IPAndNamesList, string>();
            listGenerator.Modify();
            /*Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("SSH", new SSHMenu())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title:"Menu", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);*/
        }
    }
}