using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    public class SearchResult
    {
        public IEnumerable<Track> TopTracks { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
    }
}
