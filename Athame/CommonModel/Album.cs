using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.CommonModel
{
    public class Album
    {
        public string Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public Uri CoverUri { get; set; }
        public List<Track> Tracks { get; set; }

        public int? GetNumberOfTracksOnDisc(int disc)
        {
            if (Tracks == null) return null;
            return (from t in Tracks
                    where t.DiscNumber == disc
                    select t).Count();
        }

        public int? GetTotalDiscs()
        {
            if (Tracks == null) return null;
            var totalDiscs = 0;
            foreach (var track in Tracks)
            {
                if (track.DiscNumber > totalDiscs)
                {
                    totalDiscs++;
                }
            }
            return totalDiscs;
        }

    }
}
