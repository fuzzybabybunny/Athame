using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;

namespace Athame.Utils
{
    public static class TrackFileExtensions
    {
        public static string GetPath(this TrackFile trackFile, string pathFormat)
        {
            var cleanedFilePath = StringObjectFormatter.Format(pathFormat, trackFile.Track,
                o => PathHelpers.CleanFilename(o.ToString()));
            return $"{cleanedFilePath}.{trackFile.FileType.Extension}";
        }
    }
}
