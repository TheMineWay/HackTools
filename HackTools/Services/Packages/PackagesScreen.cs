using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace HackTools
{
    class PackagesScreen : ScreenOption
    {
        public override void Open()
        {
            Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("Available packages", new PackagesListScreen(), enabled: File.Exists(ProgramInfo.programDir + "/maven.index.json")),
                new Menu<ScreenOption>.MenuItem("Fetch packages", new FetchPackagesScreen())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title: "Package manager", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);
        }
    }

    class FetchPackagesScreen : ScreenOption
    {
        public override void Open()
        {
            Console.Clear();
            LoadingUI loadingUI = new LoadingUI($"Fetching {ProgramInfo.repository}");
            WebClient client = new WebClient();
            Task<string> data = Task.Run(() => client.DownloadString(ProgramInfo.repository + "/index.json"));
            do
            {
                loadingUI.Print();
                System.Threading.Thread.Sleep(200);
            } while (!data.IsCompleted);

            // Once its downloaded

            File.WriteAllText($@"{ProgramInfo.programDir}/maven.index.json", data.Result); // <-- Save the package

            MavenIndexModel.CheckVersion(); // <-- Check if there is a newer version of the program

            Console.Clear();
            Printer.Print("&green;\nThe package index has been fetched!");
            GC.Collect();
            UIComponents.PressAnyKey();
        }
    }

    class PackagesListScreen : ScreenOption
    {
        public override void Open()
        {
            // Read packages index
            MavenIndexModel indexModel = MavenIndexModel.Get();

            Menu<string>.MenuItem[] items = indexModel.packages.Select((p) => new Menu<string>.MenuItem($"{p.name} - {p.last_version}",$"https://maven.themineway.org/repository/hacktools/packages/{p.name}/{p.last_version}")).ToArray();
            Menu<string> menu = new Menu<string>(title: "Available packages", items: items);
            do
            {
                string url = menu.DisplayMenu();
                if (url == null) return;
            } while (true);
        }
    }
}