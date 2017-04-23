using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI;
using Athame.PluginAPI.Service;

namespace Athame.Plugin
{
    public class PluginManager
    {
        public const string PluginDir = "Plugins";
        public const string PluginDllPrefix = "AthamePlugin.";

        private readonly string pluginDir;

        public PluginManager(string pluginDir)
        {
            this.pluginDir = pluginDir;
            Directory.CreateDirectory(pluginDir);
            Plugins = new List<IPlugin>();
            Services = new ServiceRegistry();
        }

        public List<IPlugin> Plugins { get; }

        public ServiceRegistry Services { get; }

//        private Type[] LoadAssemblies()
//        {
//            var directories = Directory.GetDirectories(pluginDir, PluginDllPrefix + "*");
//
//        }

        public void LoadAll()
        {
            var pluginDlls = Directory.GetFiles(pluginDir, "*.dll");
            // Load and activate all plugins
            var assemblies = from dllPath in pluginDlls
                where Path.GetFileName(dllPath).StartsWith(PluginDllPrefix)
                select Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), dllPath));
            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;
                foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                {
                    Assembly.Load(referencedAssembly);
                }
                var types = assembly.GetExportedTypes();
                // Only filter for types which can be instantiated and implement IPlugin somehow.
                var implementingType = types.FirstOrDefault(
                    type => 
                        !type.IsInterface && 
                        !type.IsAbstract && 
                        type.GetInterface(nameof(IPlugin)) != null);
                if (implementingType == null)
                {
                    throw new PluginLoadException("No exported types found implementing IPlugin.",
    assembly.Location);
                }
                // Activate base plugin
                var plugin = (IPlugin) Activator.CreateInstance(implementingType);
                plugin.Init(Program.DefaultApp);
                Plugins.Add(plugin);
                // If it's a service plugin, add it to main service collection
                var servicePlugin = plugin as IServicePlugin;
                if (servicePlugin == null) return;
                var service = servicePlugin.CreateMusicService();
                Services.Register(service);
            }
        }
    }
}
