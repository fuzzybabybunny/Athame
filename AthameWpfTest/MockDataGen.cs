using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace AthameWPF
{
    public struct Limit
    {
        private Random random;

        public Limit(int lower, int upper)
        {
            Lower = lower;
            Upper = upper;
            random = new Random();
        }

        public int Lower { get; set; }
        public int Upper { get; set; }

        public int RandomValue(bool lowerInclusive = true, bool upperInclusive = true)
        {
            return random.Next(lowerInclusive ? Lower : Lower - 1, upperInclusive ? Upper : Upper - 1);
        }

        public IList<T> RandomSet<T>(IList<T> array, bool lowerInclusive = true, bool upperInclusive = true)
        {
            var value = RandomValue(lowerInclusive, upperInclusive);
            var list = new List<T>(value);
            for (var i = 0; i < value; i++)
            {
                list.Add(array[random.Next(array.Count)]);
            }
            return list;
        }
    }

    public static class MockDataGen
    {
        private static readonly string[] ArtistNames = {
            "Jerilyn Liles", "Keena Ardrey", "Zena Vandyne", "Merlene Tam", "Marine Trogdon", "Franklin Kaminski",
            "Lawrence Parise", "Katrice Lindsley", "Marylou Shaul", "Marco Morrisette", "Amal Roux", "Yuki Mattinson",
            "Dimple Sholar", "Ayana Lichty", "Abbey Stalvey", "Anastasia Faucette", "Shanti Lady", "Hulda Overall",
            "Paris Westbrook", "Christiane Vazguez"
        };

        private static readonly string[] AlbumWords =
        {
            "hungry", "mailbox", "dazzling", "stage", "basin", "noise", "fancy", "labored", "scream", "whirl",
            "literate", "account"
        };

        private static readonly string[] TrackWords =
        {
            "rainstorm", "wistful", "elbow", "warm", "dinner", "powder",
            "fold", "sun", "brief", "cactus", "hang", "snobbish", "curve", "trashy", "grubby", "weight", "influence",
            "print", "design", "plausible", "word", "cap", "righteous", "efficient", "incandescent", "top", "tasty",
            "bare", "domineering", "hands"
        };

        public static Album GenerateAlbum()
        {
            var tracksPerAlbum = new Limit(8, 20);
            var wordsInAlbumTitle = new Limit(1, 3);
            var wordsInTrackTitle = new Limit(1, 6);

            var trackCount = tracksPerAlbum.RandomValue();
            var artistRng = new Random();
            var artistName = ArtistNames[artistRng.Next(ArtistNames.Length)];

            var album = new Album
            {
                Artist = new Artist { Name = artistName },
                Id = Guid.NewGuid().ToString(),
                Title = ToTitleCase(String.Join(" ", wordsInAlbumTitle.RandomSet(AlbumWords))),
                Tracks = new List<Track>(),
                CoverUri = new Uri("https://placehold.it/256")
            };

            for (var i = 0; i < trackCount; i++)
            {
                album.Tracks.Add(new Track
                {
                    Album = album,
                    Artist = new Artist { Name = artistName },
                    DiscNumber = 1,
                    TrackNumber = i + 1,
                    Id = Guid.NewGuid().ToString(),
                    Title = ToTitleCase(String.Join(" ", wordsInTrackTitle.RandomSet(TrackWords)))
                });
            }

            return album;
        }

        private static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
