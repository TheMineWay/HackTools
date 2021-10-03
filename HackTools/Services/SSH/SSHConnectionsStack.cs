using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HackTools
{
    class SSHConnectionsStackScreen
    {
        enum Options
        {
            exit,
            runCommand
        }
        SSHConnection[] connections;
        public SSHConnectionsStackScreen(SSHConnection[] connections)
        {
            this.connections = connections;
        }
        public void Open()
        {
            Menu<Options>.MenuItem[] items = new Menu<Options>.MenuItem[]
            {
                new Menu<Options>.MenuItem("Run command on all devices", Options.runCommand)
            };
            Menu<Options> menu = new Menu<Options>(title: "Operations", items: items);
            do
            {
                switch(menu.DisplayMenu())
                {
                    case Options.runCommand:
                        Printer.Print("&cyan;\nCommand: ",newLine: false);
                        string command = Console.ReadLine();
                        if (command == "") break;

                        foreach (SSHConnection connection in connections)
                        {
                            connection.Connect(false);
                            string results = connection.Run(new string[] { command });
                        }

                        Printer.Print("&cyan;Display all results?");
                        if (UIComponents.GetYesNo())
                        {
                            Console.Clear();
                            Printer.Print($"&white;Results of the execution of &cyan;{command}&white; on &cyan;{connections.Length}&white; devices.");
                            foreach (SSHConnection connection in connections)
                            {
                                Printer.Print($"&white;Device: &green;{connection.ip}&white; - User: &green;{connection.username}");
                                if (connection.Results.Length > 0)
                                    Console.WriteLine(connection.Results[connection.Results.Length - 1]);
                            }
                            UIComponents.PressAnyKey();
                        }
                        break;
                    default:
                        Printer.Print("&cyan;[?] Are you sure you want to exit?");
                        if (UIComponents.GetYesNo()) return;
                        break;
                }
            } while (true);
        }
    }
}