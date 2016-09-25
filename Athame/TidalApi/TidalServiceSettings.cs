using Athame.CommonModel;
using OpenTidl.Enums;

namespace Athame.TidalApi
{
    public class TidalServiceSettings : StoredSettings
    {
        public SoundQuality StreamQuality { get; set; }

        public TidalServiceSettings()
        {
            StreamQuality = SoundQuality.HIGH;
        }
    }
}
