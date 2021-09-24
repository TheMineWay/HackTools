using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;

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
            Printer.Print("&cyan;\tDestination: ", newLine: false);
            string ip;
            do
            {
                ip = Console.ReadLine();
                Console.Clear();
                Printer.Print("&blue;\t[-] Checking");
            } while (true);
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