using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Service
{
    public class SingleTrackCollection : IMediaCollection
    {
        internal SingleTrackCollection(Track t)
        {
            Tracks = new List<Track> { t };
            Title = t.Title;
            Id = t.Id;
        }

        public IList<Track> Tracks { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
    }
}
