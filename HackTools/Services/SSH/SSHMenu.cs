using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class SSHMenu: ScreenOption
    {
        enum Options
        {
            exit,
            connect
        }
        public override void Open()
        {
            Menu<Options>.MenuItem[] items = new Menu<Options>.MenuItem[]
            {
                new Menu<Options>.MenuItem("Connect", Options.connect)
            };
            Menu<Options> menu = new Menu<Options>(title: "SSH Service", items: items);
            do
            {
                Options option = menu.DisplayMenu();
                if (option == Options.exit) return;

                switch(option)
                {
                    case Options.connect: Connect(); break;
                }
            } while (true);
        }

        public static void Connect()
        {
            string title = Console.Title;
            SSHConnection connection = new SSHConnection();
            if (!connection.ConnectWithUI()) return;
            if (!connection.Connect()) return;
            Console.Clear();
            Console.Title = $"[SSH]: Connected to {connection.ip}";
            do
            {
                string command = Console.ReadLine();
                if (command == "clear") Console.Clear();
                else if (command == "exit") break;
                else
                {
                    string result = connection.Run(command);
                    Console.WriteLine(result);
                }
            } while (true);
            Console.Title = title;
        }
    }
}