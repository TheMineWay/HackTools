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

        public static long[] GetRangeAsLong(string from, string to)
        {
            long fromIp = (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(from).Address);
            long toIp = (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(to).Address);

            List<long> ips = new List<long>();

            for (long i = fromIp; i <= toIp; i++) ips.Add(i);

            return ips.ToArray();
        }
    }
}