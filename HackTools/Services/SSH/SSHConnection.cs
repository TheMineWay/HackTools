using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace HackTools
{
    class SSHConnection
    {
        public string name;
        public string ip;
        public string username, password;
        public DateTime connected;
        private List<string> results = new List<string>();
        public string[] Results => results.ToArray();

        SshClient client;

        public SSHConnection(bool connectWithUI = false)
        {
            if (connectWithUI) ConnectWithUI();
        }

        public SSHConnection(string username, string password, string ip, string name = "SSH Connection")
        {
            this.username = username;
            this.password = password;
            this.ip = ip;
        }

        public ShellStream GetStream() => client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

        public bool Connect(bool displayUI = true)
        {
            try
            {
                client = new SshClient(ip, username, password);
                connected = DateTime.Now;
                client.Connect();
                return true;
            } catch (Exception e)
            {
                if(displayUI) UIComponents.Error($"Cannot connect to {ip} with:\n\tUsername: {username}\n\tPassword: {Printer.Fill("*",password.Length)}");
                client.Disconnect();
                return false;
            }
        }

        public bool ConnectWithUI()
        {
            Console.Clear();
            // UI
            string ip;
            do
            {
                Printer.Print("&cyan;\tDestination (empty to exit): ", newLine: false);
                ip = Console.ReadLine();
                if (ip == "") return false;
                Console.Clear();
                LoadingUI loading = new LoadingUI($"Checking IP ({ip})");
                PingReply reply = null;
                Task ping = Task.Run(async () => {
                    Ping ping = new Ping();
                    reply = ping.Send(ip);
                });
                int times = 0;
                while (reply == null)
                {
                    System.Threading.Thread.Sleep(200);
                    times++;
                    loading.Print();
                    if (times >= 20) break;
                }
                if (times < 20)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        Printer.Print("&green; Successful IP check");
                        do
                        {
                            Console.Clear();
                            string username = InputField.Text(true);
                            if (username == "")
                            {
                                Console.Clear();
                                break;
                            }

                            do
                            {
                                Console.Clear();
                                string password = InputField.Password(true);
                                if (password == "") break;

                                // Store credentials
                                this.ip = ip;
                                this.password = password;
                                this.username = username;
                                return true;
                            } while (true);
                        } while (true);
                    }
                } else
                {
                    Console.Clear();
                    Printer.Print("&red; [!] The IP check has failed");
                }
            } while (true);
        }

        public string Run(string command, bool display)
        {
            if (command == "") return "";
            if (client == null) return "[!] No connection";
            if (!client.IsConnected) client.Connect();
            SshCommand com = client.CreateCommand(command);
            try
            {
                com.Execute();
                results.Add(com.Result);
                return com.Result;
            } catch(Exception e)
            {
                Printer.Print($"&red;{e.Message}");
                return "";
            }
        }

        public void Run(string[] commands)
        {
            if (client == null) return;
            if (!client.IsConnected) client.Connect();

            ShellStream stream = GetStream();

            foreach(string command in commands)
            {
                if (command == "") continue;
                stream.WriteLine(command);
                string answer = ReadStream(stream);
                int index = answer.IndexOf(System.Environment.NewLine);
                answer = answer.Substring(index + System.Environment.NewLine.Length);
                Console.WriteLine(answer.Trim());
            }
        }

        private static string ReadStream(ShellStream stream)
        {
            StringBuilder result = new StringBuilder();

            string line;
            while ((line = stream.ReadLine()) != "this-is-the-end")
                result.AppendLine(line);

            return result.ToString();
        }
    }
}