using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class Encoders
    {
        public static string ToBase64(string text) => Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        public static string FromBase64(string b64) => Encoding.UTF8.GetString(Convert.FromBase64String(b64));
    }
}