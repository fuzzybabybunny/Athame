using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Downloader
{
    public class DownloadableFile
    {
        /// <summary>
        /// The URI to the file.
        /// </summary>
        public Uri DownloadUri { get; set; }
        /// <summary>
        /// The file type. This should be set using the MIME type of the file.
        /// </summary>
        public FileType FileType { get; set; }
    }
}
