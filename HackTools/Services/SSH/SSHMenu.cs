using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class SSHMenu: ScreenOption
    {
        enum Options
        {
            exit,
            connect
        }
        public override void Open()
        {
            Menu<Options>.MenuItem[] items = new Menu<Options>.MenuItem[]
            {
                new Menu<Options>.MenuItem("Connect", Options.connect)
            };
            Menu<Options> menu = new Menu<Options>(title: "SSH Service", items: items);
            do
            {
                Options option = menu.DisplayMenu();
                if (option == Options.exit) return;

            } while (true);
        }
    }
}