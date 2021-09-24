using System;
using System.Collections.Generic;

namespace HackTools
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadingUI loadingUI = new LoadingUI("Cargando");
            while (true)
            {
                System.Threading.Thread.Sleep(200);
                loadingUI.Print();
            }
            Menu<ScreenOption>.MenuItem[] items = new Menu<ScreenOption>.MenuItem[] {
                new Menu<ScreenOption>.MenuItem("SSH", new SSHMenu())
            };
            Menu<ScreenOption> menu = new Menu<ScreenOption>(title:"Menu", items: items);
            do
            {
                ScreenOption option = menu.DisplayMenu();
                if (option == null) return;
                option.Open();
            } while (true);
        }
    }
}