using System;
using System.Collections.Generic;
using System.Net;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            ListGenerator<IPList, IPAddress> listGenerator = new ListGenerator<IPList, IPAddress>();
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