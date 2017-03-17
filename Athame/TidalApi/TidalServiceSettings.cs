using Athame.PluginAPI.Service;
using OpenTidl.Enums;

namespace Athame.TidalApi
{
    public class TidalServiceSettings : StoredSettings
    {
        public SoundQuality StreamQuality { get; set; }
        public bool AppendVersionToTrackTitle { get; set; }
        public bool DontAppendAlbumVersion { get; set; }
        public bool UseOfflineUrl { get; set; }

        public TidalServiceSettings()
        {
            StreamQuality = SoundQuality.HIGH;
            AppendVersionToTrackTitle = true;
            DontAppendAlbumVersion = true;
            UseOfflineUrl = true;
        }
    }
}
