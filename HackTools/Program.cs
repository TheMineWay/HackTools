using System;
using System.Collections.Generic;
using System.IO;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Hack Tools - By TheMineWay";
            Console.ForegroundColor = ConsoleColor.White;

            if (Console.WindowWidth >= 76)
            {
                UIComponents.CenterAndPrint("\n██╗░░██╗░█████╗░░█████╗░██╗░░██╗  ████████╗░█████╗░░█████╗░██╗░░░░░░██████╗\n██║░░██║██╔══██╗██╔══██╗██║░██╔╝  ╚══██╔══╝██╔══██╗██╔══██╗██║░░░░░██╔════╝\n███████║███████║██║░░╚═╝█████═╝░  ░░░██║░░░██║░░██║██║░░██║██║░░░░░╚█████╗░\n██╔══██║██╔══██║██║░░██╗██╔═██╗░  ░░░██║░░░██║░░██║██║░░██║██║░░░░░░╚═══██╗\n██║░░██║██║░░██║╚█████╔╝██║░╚██╗  ░░░██║░░░╚█████╔╝╚█████╔╝███████╗██████╔╝\n╚═╝░░╚═╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝  ░░░╚═╝░░░░╚════╝░░╚════╝░╚══════╝╚═════╝░".Split("\n"));
                Printer.Print("&white;" + UIComponents.Center("by TheMineWay"));

                Printer.Print("&cyan;\n" + UIComponents.Center("Press any key to start"));
                Console.ReadKey();
            }

            if (!GenerateBaseFiles()) return;
            Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("Available tools", new StartMenuScreen()),
                new Menu<ScreenOption>.MenuItem("Package manager", new PackagesScreen()),
                new Menu<ScreenOption>.MenuItem("Program info", new InfoScreen())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title: "Menu", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);
        }

        static bool GenerateBaseFiles()
        {
            try
            {
                Directory.CreateDirectory(ProgramInfo.programDir);
                Directory.CreateDirectory(@$"{ProgramInfo.programDir}\packages");
                return true;
            } catch(Exception e)
            {
                UIComponents.Error(e.Message, clear: true);
                return false;
            }
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

    class InfoScreen : ScreenOption
    {
        public override void Open()
        {
            Console.Clear();
            Printer.Print("&green; Information");
            Console.WriteLine(Printer.Fill("-", Console.WindowWidth));
            Printer.Print("&white;The purpose of this program is to help people when testing their networks (specially at the security spectrum)." +
                " Of course, this program is Open Source, so you can view the source coude on its GitHub repository (&cyan;https://github.com/TheMineWay/HackTools&white;)." +
                " Feel free to modify and redistribute copies of this software.");

            Printer.Print("&white;All contributions to the source code are welcome &red;<3&white;");
            UIComponents.PressAnyKey();
        }
    }
}