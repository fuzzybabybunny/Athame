using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTidl.Models;

namespace Athame.TidalApi
{
    internal class EnglishArtistNameJoiner
    {
        private const string LastTwoItemsJoin = " & ";
        private const string Joiner = " , ";
        private const string FeaturingWord = "feat.";

        public const string ArtistMain = "MAIN";
        public const string ArtistFeatured = "FEATURED";

        public static string JoinArtistNames(string[] artistNames)
        {
            if (artistNames.Length == 1)
            {
                return artistNames[0];
            }
           
            if (artistNames.Length == 2)
            {
                return String.Join(LastTwoItemsJoin, artistNames);
            }
            var sb = new StringBuilder();
            for (var i = 0; i < artistNames.Length; i++)
            {
                var last = artistNames.Length - 1;
                var isLast = last == -i;
                var isSecondToLast = last - 1 == i;
                sb.Append(artistNames[i]);
                if (!isLast)
                {
                    sb.Append(!isSecondToLast ? Joiner : LastTwoItemsJoin);
                }
            }
            return sb.ToString();
        }

        public static string JoinFeaturingArtists(string[] artistNames)
        {
            return String.Format("({0} {1})", FeaturingWord, JoinArtistNames(artistNames));
        }

        public static bool DoesTitleContainArtistString(TrackModel track)
        {
            var featuringArtists = (from a in track.Artists
                where a.Type == ArtistFeatured
                select a.Name).ToArray();
            var artistString = JoinFeaturingArtists(featuringArtists);
            return track.Title.Contains(artistString);
        }

        public static string CreateArtistString(TrackModel track)
        {
            var mainArtists = new List<string>();
            var featuringArtists = new List<string>();

            foreach (var artist in track.Artists)
            {
                if (artist.Type == ArtistMain)
                {
                    mainArtists.Add(artist.Name);
                }
                else
                {
                    featuringArtists.Add(artist.Name);
                }
            }

            if (DoesTitleContainArtistString(track))
            {
                return JoinArtistNames(mainArtists.ToArray());
            }
            return JoinArtistNames(mainArtists.ToArray()) + " " + JoinFeaturingArtists(featuringArtists.ToArray());
            
        }


    }
}
