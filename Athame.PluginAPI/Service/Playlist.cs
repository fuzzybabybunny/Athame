using System;
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

        public string Id { get; set; }

        /// <summary>
        /// The tracks contained within the playlist.
        /// </summary>
        public IList<Track> Tracks { get; set; }

        /// <summary>
        /// The date of when the playlist was initially published. May be null.
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// The date of when the playlist was last modified. May be null.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }
    }
}
