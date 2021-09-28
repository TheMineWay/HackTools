using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace HackTools
{
    class SSHMenu: ScreenOption
    {
        enum Options
        {
            exit,
            connect,
            attack
        }
        enum AttackOptions
        {
            exit,
            editList,
            generateRange
        }
        public override void Open()
        {
            Menu<Options>.MenuItem[] items = new Menu<Options>.MenuItem[]
            {
                new Menu<Options>.MenuItem("Connect", Options.connect),
                new Menu<Options>.MenuItem("Attack", Options.attack)
            };
            Menu<Options> menu = new Menu<Options>(title: "SSH Service", items: items);
            do
            {
                Options option = menu.DisplayMenu();
                if (option == Options.exit) return;

                switch(option)
                {
                    case Options.connect: Connect(); break;
                    case Options.attack: Attack(); break;
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

        public static void Attack()
        {
            ListGenerator<IPAndNamesList, string> listGenerator = new ListGenerator<IPAndNamesList, string>();
            Menu<AttackOptions>.MenuItem[] items = new Menu<AttackOptions>.MenuItem[] {
                new Menu<AttackOptions>.MenuItem("Edit targets list", AttackOptions.editList),
                new Menu<AttackOptions>.MenuItem("Generate targets range (by IP)", AttackOptions.generateRange)
            };
            Menu<AttackOptions> menu = new Menu<AttackOptions>(title: "Targets", items: items);
            do
            {
                AttackOptions option = menu.DisplayMenu();
                switch(option)
                {
                    case AttackOptions.editList: listGenerator.Modify(); break;
                    case AttackOptions.generateRange: AddIPRange(); break;
                    default:
                        listGenerator = null;
                        return;
                }
            } while (true);

            void AddIPRange()
            {
                Console.Clear();
                string startIp = "", endIp = "";
                do
                {
                    Printer.Print("&cyan;\nStarting IP:&white; ", newLine: false);
                    startIp = Console.ReadLine();
                    if (startIp.Length <= 0) return;
                    do
                    {
                        Printer.Print("&cyan;\nEnding IP:&white; ", newLine: false);
                        endIp = Console.ReadLine();
                        if (endIp.Length <= 0) break;
                        try
                        {
                            // Generate the range and convert it
                            listGenerator.AddRange(
                                IPWorker.GetRange(startIp, endIp).Select((e) => {
                                    IPAndNamesList newIp = new IPAndNamesList();
                                    newIp.SetBoth(e);
                                    return newIp;
                                }).ToArray());
                            return;
                        } catch(Exception e)
                        {
                            UIComponents.Error($"Invalid IP range ({startIp} - {endIp})");
                            return;
                        }
                    } while (true);
                    Console.Clear();
                } while (true);
            }
        }
    }
}