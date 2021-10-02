using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HackTools
{
    [System.Serializable]
    class MavenIndexModel
    {
        public string version;
        public MavenPackageModel[] packages;
    }

    [System.Serializable]
    class MavenPackageModel
    {
        public string name;
        public string last_version;
    }
}