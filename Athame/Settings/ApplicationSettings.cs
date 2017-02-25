using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Athame.Settings
{
    public class SettingsManager<T> where T : new()
    {
        private const string SettingsFilename = "settings.json";

        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Athame");

        private static readonly string SettingsPath = Path.Combine(SettingsDirectory, SettingsFilename);

        private readonly JsonSerializerSettings SerializerSettings;

        public T Settings { get; private set; }

        public SettingsManager()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }

        public void Load()
        {
            Directory.CreateDirectory(SettingsDirectory);
            if (!File.Exists(SettingsPath))
            {
                Settings = new T();
            }
            else
            {
                // Assign settings path to deserialised settings instance
                Settings = JsonConvert.DeserializeObject<T>(File.ReadAllText(SettingsPath),
                    SerializerSettings);
            }
        }

        public void Save()
        {
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Settings, SerializerSettings));
        }
    }
}
