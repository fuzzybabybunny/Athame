using System;
using System.Collections.Generic;
using System.Linq;
using Athame.PlayMusicApi;
using Athame.PluginAPI.Service;
using Athame.TidalApi;

namespace Athame
{
    /// <summary>
    /// Represents a collection of services and provides methods to retrieve services by name or host.
    /// </summary>
    public class ServiceCollection : List<MusicService>
    {
        private static ServiceCollection _inst;
        private Dictionary<Uri, MusicService> servicesByUri = new Dictionary<Uri, MusicService>();

        public static ServiceCollection Default => _inst ?? (_inst = new ServiceCollection());

        static ServiceCollection()
        {
            // Built-ins
            Default.Add(new PlayMusicService());
            Default.Add(new TidalService());
            

            // Load settings for each service
            var storedServiceSettings = ApplicationSettings.Default.ServiceSettings;
            foreach (var service in Default)
            {
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
                    Default.servicesByUri.Add(uri, service);
                }
            }
        }

        public MusicService GetByName(string name)
        {
            return (from s in this
                where s.Name == name
                select s).FirstOrDefault();
        }

        [Obsolete]
        public MusicService GetByHost(string host)
        {
            return (from s in this
                where s.WebHost == host
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
