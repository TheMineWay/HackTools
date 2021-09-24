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

        public SSHConnection(string username, string password, string ip, string name = "SSH Connection")
        {
            this.username = username;
            this.password = password;
            this.ip = ip;
        }

        public void Connect()
        {
            //ConnectionInfo connectionInfo = new ConnectionInfo(ip, username, new PasswordAuthenticationMethod(username,password));
            //client = new SftpClient(connectionInfo);
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