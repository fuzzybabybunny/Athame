using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMusicApi.Structure;

namespace Athame.CommonModel
{
    public class SearchResult
    {
        public IEnumerable<Track> TopTracks { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
    }
}
