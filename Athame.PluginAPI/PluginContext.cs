using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI
{
    public class PluginContext
    {
        public PluginContext(string dataDirectory)
        {
            DataDirectory = dataDirectory;
        }
        public string DataDirectory { get; private set; }
    }
}
