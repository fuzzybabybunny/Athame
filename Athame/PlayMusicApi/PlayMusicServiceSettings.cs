using Athame.CommonModel;
using GoogleMusicApi.Structure.Enums;

namespace Athame.PlayMusicApi
{
    public class PlayMusicServiceSettings : StoredSettings
    {
        public StreamQuality StreamQuality { get; set; }

        public PlayMusicServiceSettings()
        {
            StreamQuality = StreamQuality.High;
        }
    }
}
