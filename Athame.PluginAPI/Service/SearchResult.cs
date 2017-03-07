using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    public class SearchResult
    {
        public PaginatedRecordSet<Track> TopTracks { get; set; }
        public PaginatedRecordSet<Album> Albums { get; set; }
        public PaginatedRecordSet<Artist> Artists { get; set; }
        public PaginatedRecordSet<Playlist> Playlists { get; set; }
    }
}
