using System.Collections.Generic;
using System.Linq;
using Athame.PlayMusicApi;
using Athame.TidalApi;

namespace Athame.CommonModel
{
    /// <summary>
    /// Represents a collection of services and provides methods to retrieve services by name or host.
    /// </summary>
    public class ServiceCollection : List<Service>
    {
        private static ServiceCollection _inst;

        public static ServiceCollection Default
        {
            get { return _inst ?? (_inst = new ServiceCollection()); }
        }

        static ServiceCollection()
        {
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
            }
        }

        public Service GetByName(string name)
        {
            return (from s in this
                where s.Name == name
                select s).FirstOrDefault();
        }

        public Service GetByHost(string host)
        {
            return (from s in this
                where s.WebHost == host
                select s).FirstOrDefault();
        }
    }
}
