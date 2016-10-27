using System.Collections.Generic;
using System.Linq;
using Athame.PluginAPI.Service;

namespace Athame.InternalModel
{
    public class DownloadableMediaCollection
    {
        public Service Service { get; set; }
        public string Name { get; set; }
        public MediaType CollectionType { get; set; }
        public List<DownloadableTrack> Tracks { get; set; }
        public string Id { get; set; }

        public DownloadableMediaCollection()
        {
            
        }

        public DownloadableMediaCollection(string pathFormat, IEnumerable<Track> commonTracks)
        {
            Tracks = new List<DownloadableTrack>(from t in commonTracks
                select DownloadableTrack.FromCommon(pathFormat, t));
        }
    }
}
