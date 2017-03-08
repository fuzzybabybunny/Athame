using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace Athame.PluginAPI
{
    /// <summary>
    /// Represents a music service plugin.
    /// </summary>
    public interface IServicePlugin : IPlugin
    {
        /// <summary>
        /// Creates a new instance of a <see cref="MusicService"/>. This method is only called once, and after <see cref="IPlugin.Init"/>.
        /// </summary>
        /// <returns>A new <see cref="MusicService"/>.</returns>
        MusicService CreateMusicService();
    }
}
