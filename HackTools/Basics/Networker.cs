using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HackTools
{
    class Networker
    {
        public static void DownloadToFile(string url, string destination)
        {
            WebClient client = new WebClient();
            if (File.Exists(destination)) File.Delete(destination);

            Stream ws = client.OpenRead(url);
            using (FileStream fs = new FileStream(destination, FileMode.Create))
            {
                ws.CopyTo(fs);
                fs.Close();
            }
            ws.Close();
        }
    }
}