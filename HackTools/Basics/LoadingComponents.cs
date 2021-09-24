using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HackTools
{
    class LoadingUI
    {
        public enum Mode
        {
            spinning,
            horizontal
        }
        public static Mode mode = Mode.horizontal;
        string title;
        public LoadingUI(string title)
        {
            this.title = title;
        }

        int pos = 0;
        int dir = 1;
        public void Print(bool clear = true)
        {
            if (clear) Console.Clear();
            string h;
            if (mode == Mode.spinning)
            {
                h = pos == 0 ? "-" : (pos == 1 ? "/" : (pos == 2 ? "|" : "\\"));
                pos++;
                if (pos <= 0 || pos >= 4) pos = 0;
            } else
            {
                h = pos == 0 ? "0oo" : (pos == 1 ? "o0o" : "oo0");
                pos += 1 * dir;
                if (pos <= 0 || pos >= 2) dir *= -1;
            }
            if (pos <= 0 || pos >= 4) pos = 0;
            Printer.Print($"&cyan; [&green;{h}&cyan;]&white; {title}");
        }
    }
}