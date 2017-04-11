using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.PluginAPI.Service;

namespace Athame.Settings
{
    public enum AlbumArtworkSaveFormat
    {
        DontSave,
        AsCover,
        AsArtistAlbum
    }

    public class MediaTypeSavePreference
    {
        public string SaveDirectory { get; set; }
        public string SaveFormat { get; set; }
        public bool AskForLocation { get; set; }

        public string GetPlatformSaveFormat()
        {
            return Path.DirectorySeparatorChar == '/' ? SaveFormat : SaveFormat.Replace('/', Path.DirectorySeparatorChar);
        }
    }

    public class WindowPreference
    {
        public Point Location { get; set; }
        public Size Size { get; set; }

        public void CopyFrom(Form form)
        {
            Location = form.Location;
            Size = form.Size;
        }

        public void CopyTo(Form form)
        {
            form.Location = Location;
            form.Size = Size;
        }
    }

    public class AthameSettings
    {
        // Defaults
        public AthameSettings()
        {
            ServiceSettings = new Dictionary<string, StoredSettings>();
            AlbumArtworkSaveFormat = AlbumArtworkSaveFormat.AsCover;
            GeneralSavePreference = new MediaTypeSavePreference
            {
                AskForLocation = false,
                SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                SaveFormat = "{AlbumArtistOrArtist} - {Album.Title}/{TrackNumber} {Title}"
            };
            PlaylistSavePreference = new MediaTypeSavePreference
            {
                AskForLocation = false,
                SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                SaveFormat = "{Playlist}/{AlbumArtistOrArtist} - {Title}"
            };
            PlaylistSavePreferenceUsesGeneral = true;
            MainWindowPreference = new WindowPreference();
        }

        public Dictionary<string, StoredSettings> ServiceSettings { get; set; }
        public AlbumArtworkSaveFormat AlbumArtworkSaveFormat { get; set; }
        public MediaTypeSavePreference GeneralSavePreference { get; set; }
        public MediaTypeSavePreference PlaylistSavePreference { get; set; }
        public bool PlaylistSavePreferenceUsesGeneral { get; set; }
        public WindowPreference MainWindowPreference { get; set; }
    }
}
