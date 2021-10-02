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
            MavenIndexModel indexModel = MavenIndexModel.GetFromFile();

            Menu<MavenPackageModel>.MenuItem[] items = getItems();
            Menu<MavenPackageModel> menu = getMenu();
            do
            {
                MavenPackageModel mavenPackage = menu.DisplayMenu();
                if (mavenPackage == null) return;
                bool downloaded = mavenPackage.View();
                if (downloaded)
                {
                    items = getItems();
                    menu = getMenu();
                }
            } while (true);

            Menu<MavenPackageModel>.MenuItem[] getItems() => indexModel.packages.Select((p) => new Menu<MavenPackageModel>.MenuItem($"{p.name} ({p.last_version}) {(p.IsAvailable() ? "" : "[DOWNLOAD]")}", p)).ToArray();
            Menu<MavenPackageModel> getMenu() => new Menu<MavenPackageModel>(title: "Available packages", items: items);
        }
    }
}