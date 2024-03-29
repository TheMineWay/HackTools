﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace HackTools
{
    class IOWorker
    {
        enum IOStatus
        {
            exit,
            returning,
            back
        }
        public static FileInfo FileBrowser(DirectoryInfo startAt = null)
        {
            DirectoryInfo startPath = startAt == null ? new DirectoryInfo(Directory.GetCurrentDirectory()) : startAt;
            FileSystemInfo fsi = GetElement(startPath, canSelectFiles: true);
            return fsi == null ? null : new FileInfo(fsi.FullName);
        }
        public static DirectoryInfo DirectoryBrowser(DirectoryInfo startAt = null)
        {
            DirectoryInfo startPath = startAt == null ? new DirectoryInfo(Directory.GetCurrentDirectory()) : startAt;
            FileSystemInfo fsi = GetElement(startPath, canSelectFolders: true, canSelectFiles: false);
            return fsi == null ? null : new DirectoryInfo(fsi.FullName);
        }

        public static FileInfo NewFileBrowser(DirectoryInfo startAt = null)
        {
            DirectoryInfo startPath = startAt == null ? new DirectoryInfo(Directory.GetCurrentDirectory()) : startAt;
            do
            {
                FileSystemInfo fsi = GetElement(startPath, canSelectFolders: true, canSelectFiles: false);
                if (fsi == null) return null;

                Printer.Print("&cyan;Name for the new file (empty to exit): ", newLine: false);
                string name = Console.ReadLine();
                if (name == "")
                {
                    startPath = new DirectoryInfo(fsi.FullName).Parent;
                    continue;
                }
                DirectoryInfo dir = new DirectoryInfo(fsi.FullName);
                if (dir.GetFiles().Any((f) => f.Name == name))
                {
                    UIComponents.Error($"There is already a file using the name \"{name}\".", clear: true);
                    continue;
                }

                return new FileInfo(dir.FullName + @"\" + name);
            } while (true);
        }

        private static FileSystemInfo GetElement(DirectoryInfo directory, bool displayFiles = true, bool canSelectFolders = false, bool canSelectFiles = false)
        {
            DirectoryInfo current = directory;
            List<IOItem> IOItems = new List<IOItem>();

            bool update = true;
            int selected = 0;
            int perPage;
            int page;
            do
            {
                if(update)
                {
                    try
                    {
                        IOItems.Clear();
                        selected = 0;
                        IOItems.AddRange(current.GetDirectories().Select((d) => new IOItem(d, true)).ToArray());
                        IOItems.AddRange((displayFiles ? current.GetFiles() : new FileInfo[] { }).Select((f) => new IOItem(f, false)).ToArray());
                        update = false;
                    } catch(Exception e)
                    {
                        UIComponents.Error("An error has occurred while reading the directory");
                        current = current.Parent;
                        continue;
                    }

                }
                perPage = Console.WindowHeight - 12;
                if (perPage < 3) perPage = 3;
                page = selected / perPage;

                Console.Clear();
                Printer.Print($"&white;Page &red;{page + 1}/{(IOItems.Count / perPage) + 1}");
                Console.WriteLine(Printer.Fill("-", Console.WindowWidth));
                if(IOItems.Count <= 0) Printer.Print("&cyan;The directory is empty\n", newLine: false);
                for (int i = page * perPage; i < IOItems.Count && i < (page * perPage) + perPage; i++)
                {
                    IOItem item = IOItems[i];
                    Printer.Print($"&red;{(selected == i ? "$ " : "  ")}{(item.isDirectory ? "&white;" : "&yellow;")}{item.fileSystemInfo.Name}\n", newLine: false);
                }
                Console.WriteLine(Printer.Fill("-", Console.WindowWidth));

                // Actions
                Printer.Print("&cyan;Enter (directory): &white;Navigate\t&cyan;R: &white;Exit\n&cyan;Enter (file): &white;Select\t\t&cyan;S: &white;Select");

                IOItem selectedItem = IOItems.Count <= 0 ? null : IOItems[selected];
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow: selected--; break;
                    case ConsoleKey.DownArrow: selected++; break;
                    case ConsoleKey.Enter:
                        if (selectedItem == null) break;
                        if (!selectedItem.isDirectory && !canSelectFiles) break;
                        if (selectedItem.isDirectory)
                        {
                            current = new DirectoryInfo(selectedItem.fileSystemInfo.FullName);
                            update = true;
                            break;
                        }
                        Console.Clear();
                        return selectedItem.fileSystemInfo;
                    case ConsoleKey.S:
                        if (selectedItem == null) break;
                        if (selectedItem.isDirectory && !canSelectFolders) break;
                        if (!selectedItem.isDirectory && !canSelectFiles) break;
                        Console.Clear();
                        return selectedItem.fileSystemInfo;
                        break;
                    case ConsoleKey.Escape:
                        if (current.Parent == null) break;
                        current = current.Parent;
                        update = true;
                        break;
                    case ConsoleKey.R:
                        Console.Clear();
                        return null;
                }

                // Correct
                if (selected < 0) selected = IOItems.Count - 1;
                if (selected >= IOItems.Count) selected = 0;
            } while (true);
        }

        class IOItem
        {
            public IOItem(FileSystemInfo fileSystemInfo, bool isDirectory)
            {
                this.fileSystemInfo = fileSystemInfo;
                this.isDirectory = isDirectory;
            }

            public FileSystemInfo fileSystemInfo;
            public bool isDirectory;
        }
    }
}