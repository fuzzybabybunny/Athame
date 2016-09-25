using Athame.CommonModel;
using TagLib;

namespace Athame.DownloadAndTag
{
    public class TrackTagger
    {
        private const string CopyrightText = "Respect the artists! Pay for music when you can! Downloaded with GPMDL";

        public void Write(string path, Track track, string albumArtPath)
        {
            using (var file = File.Create(path))
            {
                file.Tag.Title = track.Title;
                file.Tag.Performers = new[] {track.Artist};
                file.Tag.AlbumArtists = new[] {track.Album.Artist};
                file.Tag.Genres = new[] {track.Genre};
                file.Tag.Album = track.Album.Title;
                file.Tag.Track = (uint) track.TrackNumber;
                file.Tag.TrackCount = (uint) track.Album.GetNumberOfTracksOnDisc(track.DiscNumber);
                file.Tag.Disc = (uint) track.DiscNumber;
                file.Tag.DiscCount = (uint) track.Album.GetTotalDiscs();
                file.Tag.Year = (uint) track.Year;
                file.Tag.Copyright = CopyrightText;
                // ReSharper disable once CoVariantArrayConversion
                file.Tag.Pictures = new[]{new Picture(albumArtPath)};
                file.Save();
            }
        }
    }
}
