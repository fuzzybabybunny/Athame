using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// Represents an ordered, arbitrary list of tracks.
    /// </summary>
    public class Playlist : IMediaCollection
    {
        /// <summary>
        /// The title of the playlist.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The tracks contained within the playlist.
        /// </summary>
        public IEnumerable<Track> Tracks { get; set; }
    }
}
