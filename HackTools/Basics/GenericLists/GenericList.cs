using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class IPAndNamesList : ListItem<string>
    {
        public override bool AskForValue()
        {
            Printer.Print("&cyan;\nNew network device (IP or name):&white; ", newLine: false);
            string newIp = Console.ReadLine();
            if(newIp.Length > 0)
            {
                SetBoth(newIp);
                return true;
            }
            return false;
        }

        public void SetBoth(string val)
        {
            SetName(val);
            SetValue(val);
        }
    }
}