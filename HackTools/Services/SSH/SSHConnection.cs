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

        SshClient client;

        public SSHConnection()
        {
            // UI
            Console.Clear();
            string ip;
            do
            {
                Printer.Print("&cyan;\tDestination: ", newLine: false);
                ip = Console.ReadLine();
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
                if(times < 20)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        Printer.Print("&green; Successful IP check");
                        break;
                    }
                }
                Printer.Print("&red; [!] The IP check has failed");
            } while (true);
            // Credentials

        }

        public SSHConnection(string username, string password, string ip, string name = "SSH Connection")
        {
            this.username = username;
            this.password = password;
            this.ip = ip;
        }

        public void Connect()
        {
            client = new SshClient(ip,username,password);
            client.Connect();
        }

        public string Run(string command)
        {
            if (client == null) return "[!] No connection";
            SshCommand com = client.CreateCommand(command);
            com.Execute();
            return com.Result;
        }
    }
}