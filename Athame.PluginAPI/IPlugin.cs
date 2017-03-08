using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI
{
    /// <summary>
    /// Represents the base of all plugins.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// The plugin's name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The plugin's description.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The plugin's author.
        /// </summary>
        string Author { get; }
        /// <summary>
        /// The plugin's homepage.
        /// </summary>
        Uri Website { get; }
        /// <summary>
        /// Initialises the plugin with the specified application context.
        /// </summary>
        /// <param name="application">The application context to use.</param>
        void Init(AthameApplication application);
    }
}
