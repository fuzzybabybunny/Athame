using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.CommonModel
{
    public class Album
    {
        /// <summary>
        /// Service-specific identifier for the album. Not null.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The album artist. May be null.
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// The album's title. Not null.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The album's cover artwork URL. Not null.
        /// </summary>
        public Uri CoverUri { get; set; }
        /// <summary>
        /// The album's tracks. May be null.
        /// </summary>
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
