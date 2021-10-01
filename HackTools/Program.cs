using System;
using System.Collections.Generic;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Hack Tools - By TheMineWay";

            if (Console.WindowWidth >= 76)
            {
                UIComponents.CenterAndPrint("\n██╗░░██╗░█████╗░░█████╗░██╗░░██╗  ████████╗░█████╗░░█████╗░██╗░░░░░░██████╗\n██║░░██║██╔══██╗██╔══██╗██║░██╔╝  ╚══██╔══╝██╔══██╗██╔══██╗██║░░░░░██╔════╝\n███████║███████║██║░░╚═╝█████═╝░  ░░░██║░░░██║░░██║██║░░██║██║░░░░░╚█████╗░\n██╔══██║██╔══██║██║░░██╗██╔═██╗░  ░░░██║░░░██║░░██║██║░░██║██║░░░░░░╚═══██╗\n██║░░██║██║░░██║╚█████╔╝██║░╚██╗  ░░░██║░░░╚█████╔╝╚█████╔╝███████╗██████╔╝\n╚═╝░░╚═╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝  ░░░╚═╝░░░░╚════╝░░╚════╝░╚══════╝╚═════╝░".Split("\n"));
                Printer.Print("&white;" + UIComponents.Center("by TheMineWay"));

                Printer.Print("&cyan;\n" + UIComponents.Center("Press any key to start"));
                Console.ReadKey();
            }

            Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("Available tools", new StartMenuScreen())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title: "Menu", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);
        }
    }

    class StartMenuScreen: ScreenOption
    {
        public override void Open()
        {
            Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("SSH", new SSHMenu())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title: "Tools", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);
        }
    }
}