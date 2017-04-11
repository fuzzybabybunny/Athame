using System;
using System.Collections.Generic;
using System.Linq;
using Athame.PlayMusicApi;
using Athame.PluginAPI.Service;
using Athame.TidalApi;

namespace Athame.PluginManager
{
    /// <summary>
    /// Represents a collection of services and provides methods to retrieve services by name or host.
    /// </summary>
    public class ServiceRegistry : List<MusicService>
    {
        private readonly Dictionary<Uri, MusicService> servicesByUri = new Dictionary<Uri, MusicService>();

        private static ServiceRegistry _inst;
        public static ServiceRegistry Default => _inst ?? (_inst = new ServiceRegistry());

        static ServiceRegistry()
        {
            // Built-ins
            Default.Register(new PlayMusicService());
            Default.Register(new TidalService());
        }

        public void Register(MusicService service)
        {
            var storedServiceSettings = Program.DefaultSettings.Settings.ServiceSettings;
            // If we have saved settings for the service, restore them
            if (storedServiceSettings.ContainsKey(service.Name)
                && storedServiceSettings[service.Name] != null)
            {
                service.Settings = storedServiceSettings[service.Name];
            }
            // Otherwise, set the saved settings to the service's defaults
            else
            {
                storedServiceSettings[service.Name] = service.Settings;
            }
            foreach (var uri in service.BaseUri)
            {
                servicesByUri.Add(uri, service);
            }
            Add(service);
        }

        public MusicService GetByName(string name)
        {
            return (from s in this
                where s.Name == name
                select s).FirstOrDefault();
        }

        public MusicService GetByBaseUri(Uri baseUri)
        {
            return (from s in servicesByUri
                where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host
                select s.Value).FirstOrDefault();
        }
    }
}
