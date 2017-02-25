using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.Plugin
{
    public class PluginLoadException : Exception
    {
        public string File { get; set; }

        public PluginLoadException(string message, string file) : base(message)
        {
            File = file;
        }
    }
}
