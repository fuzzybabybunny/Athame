using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI;
using Athame.PluginAPI.MusicService;

namespace Athame.Plugin
{
    public class PluginLoader
    {
        private const string PluginDir = "Plugins";
        private const string PluginDllPrefix = "Athame.Plugin.";

        private readonly string pluginDir;

        public PluginLoader(string pluginDir)
        {
            this.pluginDir = pluginDir;
            Directory.CreateDirectory(pluginDir);
        }

        public List<IPlugin> Plugins { get; }

        public ServiceCollection Services { get; }

        private static PluginLoader _default;

        public static PluginLoader Default => _default ?? (_default = new PluginLoader(PluginDir));

        private Type[] LoadAssemblies()
        {
            var directories = Directory.GetDirectories(pluginDir, PluginDllPrefix + "*");

        }

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
                Plugins.Add(plugin);
                // If it's a service plugin, add it to main service collection
                var servicePlugin = plugin as IServicePlugin;
                if (servicePlugin == null) return;
                var service = servicePlugin.GetService();
                Services.Add(service);
            }
        }
    }
}
