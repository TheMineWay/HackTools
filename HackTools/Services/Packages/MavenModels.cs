using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace HackTools
{
    [System.Serializable]
    class MavenIndexModel
    {
        public static MavenIndexModel Get() => JsonConvert.DeserializeObject<MavenIndexModel>(File.ReadAllText($@"{ProgramInfo.programDir}/maven.index.json"));

        public static void CheckVersion()
        {
            MavenIndexModel indexModel = Get();
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

    [System.Serializable]
    class MavenPackageModel
    {
        public string name;
        public string last_version;
    }
}