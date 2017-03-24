using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace Athame.PluginAPI.Downloader
{
    /// <summary>
    /// Represents the downloadable form of <see cref="Track"/>.
    /// </summary>
    public class TrackFile : DownloadableFile
    {
        /// <summary>
        /// The track this file references.
        /// </summary>
        public Track Track { get; set; }

        /// <summary>
        /// The bit rate of the track. If not known or applicable, -1.
        /// </summary>
        public int BitRate { get; set; }
    }
}
