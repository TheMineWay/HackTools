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
            horizontalCircles,
            dots
        }
        public static Mode mode = Mode.dots;
        string title;
        public LoadingUI(string title)
        {
            this.title = title;
        }

        TextCircularQueue loadingText = new TextCircularQueue(
            mode == Mode.horizontalCircles ? new string[] { "0oo", "o0o", "oo0", "o0o" } : (mode == Mode.spinning ? new string[] { "/", "-", "\\", "|" } : new string[] { "·..", ".·.", "..·", ".·." })
        );
        public void Print(bool clear = true)
        {
            if (clear) Console.Clear();
            Printer.Print($"&cyan; [&green;{loadingText.Next()}&cyan;]&white; {title}");
        }
    }
}