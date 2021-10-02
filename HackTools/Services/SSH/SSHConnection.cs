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

        public bool Connect()
        {
            try
            {
                client = new SshClient(ip, username, password);
                connected = DateTime.Now;
                client.Connect();
                return true;
            } catch (Exception e)
            {
                UIComponents.Error($"Cannot connect to {ip} with:\n\tUsername: {username}\n\tPassword: {Printer.Fill("*",password.Length)}");
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

        public string Run(string command)
        {
            if (client == null) return "[!] No connection";
            SshCommand com = client.CreateCommand(command);
            try
            {
                com.Execute();
                return com.Result;
            } catch(Exception e)
            {
                Printer.Print("&red; Missing client");
                return "";
            }
        }
    }
}