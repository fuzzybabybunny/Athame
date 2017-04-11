using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// Represents a generic enumerable collection of tracks.
    /// </summary>
    public interface IMediaCollection
    {
        /// <summary>
        /// The tracks this collection contains.
        /// </summary>
        IList<Track> Tracks { get; set; }
        /// <summary>
        /// The human-readable title of this collection.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// The collection's ID
        /// </summary>
        string Id { get; set; }
    }
}
