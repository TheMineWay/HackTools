using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace HackTools
{
    [Serializable]
    class MavenIndexModel
    {
        public static MavenIndexModel GetFromFile() => JsonConvert.DeserializeObject<MavenIndexModel>(File.ReadAllText($@"{ProgramInfo.programDir}/maven.index.json"));

        public static void CheckVersion()
        {
            MavenIndexModel indexModel = GetFromFile();
            if (indexModel.program_version != ProgramInfo.version)
            {
                Printer.Print($"&white;[!] There is a newer version of the program available.\nDownload it here: &cyan;{indexModel.program_latest_download_url}&white; (&red;{ProgramInfo.version}&white;) => (&green;{indexModel.program_version}&white;)\n");
                UIComponents.PressAnyKey();
            }
        }

        public string program_version;
        public string program_latest_download_url;
        public string index_version;
        public MavenPackageModel[] packages;
    }

    [Serializable]
    class MavenPackageModel
    {
        public string name;
        public string last_version;
        public string url => $"https://maven.themineway.org/repository/hacktools/packages/{name}/{last_version}";
        public bool IsAvailable()
        {
            try
            {
                if (!File.Exists($@"{ProgramInfo.programDir}/packages/{name}/package.info.json")) return false;
                if (!File.Exists($@"{ProgramInfo.programDir}/packages/{name}/{MavenPackageInfo.GetFromFile(name).datafile}")) return false;
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        public bool View()
        {
            if(IsAvailable())
            {
                return false;
            } else
            {
                Task download = Task.Run(Download);
                LoadingUI loading = new LoadingUI($"Downloading {name} ({last_version}) from {url}");
                do
                {
                    loading.Print();
                    System.Threading.Thread.Sleep(200);
                } while (!download.IsCompleted);
                return true;
            }
        }

        public void Download()
        {
            WebClient client = new WebClient();

            Directory.CreateDirectory($@"{ProgramInfo.programDir}/packages/{name}");

            if (File.Exists($@"{ProgramInfo.programDir}/packages/{name}/package.info.json")) File.Delete($@"{ProgramInfo.programDir}/packages/{name}/package.info.json");
            
            Stream packageInfoStream = client.OpenRead($"{url}/v_info.json");
            using(FileStream fs = new FileStream($@"{ProgramInfo.programDir}/packages/{name}/package.info.json", FileMode.Create))
            {
                packageInfoStream.CopyTo(fs);
                fs.Close();
            }
            packageInfoStream.Close();

            if (File.Exists($@"{ProgramInfo.programDir}/packages/{name}/{MavenPackageInfo.GetFromFile(name).datafile}")) File.Delete($@"{ProgramInfo.programDir}/packages/{name}/{MavenPackageInfo.GetFromFile(name).datafile}");

            Stream dataFile = client.OpenRead($"{url}/{MavenPackageInfo.GetFromFile(name).datafile}");
            using(FileStream fs = new FileStream($@"{ProgramInfo.programDir}/packages/{name}/{MavenPackageInfo.GetFromFile(name).datafile}", FileMode.Create))
            {
                dataFile.CopyTo(fs);
                fs.Close();
            }
            dataFile.Close();
        }
    }
    [Serializable]
    class MavenPackageInfo
    {
        public static MavenPackageInfo GetFromFile(string name) => JsonConvert.DeserializeObject<MavenPackageInfo>(File.ReadAllText($@"{ProgramInfo.programDir}/packages/{name}/package.info.json"));

        public string datatype;
        public string datafile;
    }
}