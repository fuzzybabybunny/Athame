using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI
{
    public class AthameApplication
    {
        public bool IsRunningOnWin32 { get; set; }
        public bool IsWindowed { get; set; }
        public string UserDataPath { get; set; }
        public string CurrentWorkingDirectory { get; set; }
    }
}
