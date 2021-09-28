using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HackTools
{
    class IPWorker
    {
        public static string[] GetRange(string from, string to)
        {
            long fromIp = (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(from).Address);
            long toIp = (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(to).Address);

            List<string> ips = new List<string>();


            for (long i = fromIp; i <= toIp; i++) ips.Add(IPAddress.Parse(i.ToString()).ToString());

            return ips.ToArray();
        }
    }

    class IPList : ListItem<IPAddress>
    {
        public override bool AskForValue()
        {
            Printer.Print("&cyan;\nNew IP:&white; ", newLine: false);
            string newIp = Console.ReadLine();
            try
            {
                SetValue(IPAddress.Parse(newIp));
                name = newIp;
                return newIp.Length > 0;
            } catch(Exception e)
            {
                return false;
            }
        }
    }
}