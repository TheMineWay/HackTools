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
            SSHCredentials,
            stringList
        }
        public override void Open()
        {
            FileInfo file = IOWorker.FileBrowser();
            if (file == null) return;

            Menu<ListType>.MenuItem[] items = new Menu<ListType>.MenuItem[]
            {
                new Menu<ListType>.MenuItem("Simple list",ListType.stringList),
                new Menu<ListType>.MenuItem("SSH Credentials",ListType.SSHCredentials)
            };
            Menu<ListType> menu = new Menu<ListType>(title: "Select the list type", items: items);

            do
            {
                ListType type = menu.DisplayMenu();
                switch(type)
                {
                    case ListType.SSHCredentials:
                        ListGenerator<SSHConnectionsList, SSHConnection> SSHList = new ListGenerator<SSHConnectionsList, SSHConnection>();
                        try
                        {
                            SSHList.Import(file);
                            SSHList.Modify();
                        } catch(Exception e)
                        {
                            UIComponents.Error("An error has occurred while reading the file. Maybe the file does not contains a \"SSHCredentials list\".");
                        }
                        break;
                    case ListType.stringList:
                        ListGenerator<StringList, string> stringList = new ListGenerator<StringList, string>();
                        try
                        {
                            stringList.Import(file);
                            stringList.Modify();
                        }
                        catch (Exception e)
                        {
                            UIComponents.Error("An error has occurred while reading the file. Maybe the file does not contains a \"Simple List\".");
                        }
                        break;
                    default: return;
                }
            } while (true);
        }
    }
}