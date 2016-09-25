using System;
using System.Collections.Generic;
using System.IO;
using Athame.CommonModel;
using Newtonsoft.Json;

namespace Athame
{
    public class ApplicationSettings
    {
        private const string SettingsFilename = "settings.json";

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Athame");

        private static readonly string FullPath = Path.Combine(SettingsPath, SettingsFilename);

        // Need this so service settings will deserialise correctly
        private static readonly JsonSerializerSettings SerializerSettings;

        static ApplicationSettings()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }

        private static ApplicationSettings _default;

        public static ApplicationSettings Default
        {
            get
            {
                if (_default == null)
                {
                    Directory.CreateDirectory(SettingsPath);
                    if (!File.Exists(FullPath))
                    {
                        _default = new ApplicationSettings();
                        _default.Save();
                    }
                    else
                    {
                        _default = JsonConvert.DeserializeObject<ApplicationSettings>(File.ReadAllText(FullPath), SerializerSettings);
                    }
                }
                return _default;
            }
        }

        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(this, SerializerSettings));
        }

        public ApplicationSettings()
        {
            ServiceSettings = new Dictionary<string, StoredSettings>();
            SaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            TrackFilenameFormat = @"{Album.Artist} - {Album.Title}\{TrackNumber} {Title}";
        }

        public Dictionary<string, StoredSettings> ServiceSettings { get; set; }
        public string SaveLocation { get; set; }
        public string TrackFilenameFormat { get; set; }
        
    }
}
