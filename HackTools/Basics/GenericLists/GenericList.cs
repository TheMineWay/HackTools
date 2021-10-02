using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HackTools
{
    class IPAndNamesList : ListItem<long>
    {
        public override bool AskForValue()
        {
            Printer.Print("&cyan;\nNew network device (IP or name):&white; ", newLine: false);
            string newIp = Console.ReadLine();
            if(newIp.Length > 0)
            {
                try
                {
                    SetValue((uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(newIp).Address));
                    return true;
                } catch(Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public override string GetName() => IPAddress.Parse(value.ToString()).ToString();
    }
    class PasswordsList : ListItem<string>
    {
        public override bool AskForValue()
        {
            Printer.Print("&cyan;\nNew password:&white; ", newLine: false);
            string password = Console.ReadLine();
            if (password.Length > 0)
            {
                SetValue(password);
                SetName(password);
                return true;
            }
            return false;
        }
    }
    class UsernamesList : ListItem<string>
    {
        public override bool AskForValue()
        {
            Printer.Print("&cyan;\nNew username:&white; ", newLine: false);
            string username = Console.ReadLine();
            if (username.Length > 0)
            {
                SetValue(username);
                SetName(username);
                return true;
            }
            return false;
        }
    }
    class SSHConnectionsList : ListItem<SSHConnection>
    {
        public override bool AskForValue() => false;
        public override string GetName() => $"{GetValue().ip} | {GetValue().username} | {GetValue().password}";
        public override string GetExportFormat() => $"{ToHex(GetValue().ip)};{ToHex(GetValue().username)};{ToHex(GetValue().password)}";

        private string ToHex(string text) => BitConverter.ToString(Encoding.Default.GetBytes(text)).Replace("-", "");
    }
}