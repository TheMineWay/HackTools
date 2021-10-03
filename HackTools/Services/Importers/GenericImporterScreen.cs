using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HackTools
{
    class GenericImporterScreen : ScreenOption
    {
        enum ListType
        {
            exit,
            SSHCredentials
        }
        public override void Open()
        {
            FileInfo file = IOWorker.FileBrowser();
            if (file == null) return;

            Menu<ListType>.MenuItem[] items = new Menu<ListType>.MenuItem[]
            {
                new Menu<ListType>.MenuItem("SSH Credentials",ListType.SSHCredentials)
            };
            Menu<ListType> menu = new Menu<ListType>(title: "Select the list type", items: items);

            do
            {
                ListType type = menu.DisplayMenu();
                switch(type)
                {
                    case ListType.SSHCredentials:
                        ListGenerator<SSHConnectionsList, SSHConnection> listGenerator = new ListGenerator<SSHConnectionsList, SSHConnection>();
                        
                        listGenerator.Modify();
                        break;
                    default: return;
                }
            } while (true);
        }
    }
}