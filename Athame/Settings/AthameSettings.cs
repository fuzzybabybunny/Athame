using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.MusicService;

namespace Athame.Settings
{
    public class AthameSettings : SettingsManager<AthameSettings>
    {
        private static AthameSettings instance;

        public static AthameSettings Instance => instance ?? (instance = new AthameSettings());

        // Defaults
        public AthameSettings() { 
            ServiceSettings = new Dictionary<string, StoredSettings>();
            SaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            TrackFilenameFormat = "{AlbumArtistOrArtist} - {Album.Title}/{TrackNumber} {Title}";
        }

        public Dictionary<string, StoredSettings> ServiceSettings { get; set; }
        public string SaveLocation { get; set; }
        public string TrackFilenameFormat { get; set; }


    }
}
