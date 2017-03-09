using System;
using Athame.PluginAPI.Service;
using TagLib;
using SysFile = System.IO.File;

namespace Athame.DownloadAndTag
{
    public class TrackTagger
    {
        private const string CopyrightText = "Respect the artists! Pay for music when you can! Downloaded with Athame";

        private Picture CreatePictureFromCache(string url)
        {
            var data = AlbumArtCache.Instance.Get(url);
            return new Picture(new ByteVector(data, data.Length));
        }

        public void Write(string path, Track track)
        {
            using (var file = File.Create(path))
            {
                file.Tag.Title = track.Title;
                file.Tag.Performers = new[] {track.Artist};
                if (track.Album.Artist != null)
                {
                    file.Tag.AlbumArtists = new[] {track.Album.Artist};
                }
                file.Tag.Genres = new[] {track.Genre};
                file.Tag.Album = track.Album.Title;
                file.Tag.Track = (uint) track.TrackNumber;
                file.Tag.TrackCount = (uint) (track.Album.GetNumberOfTracksOnDisc(track.DiscNumber) ?? 0);
                file.Tag.Disc = (uint) track.DiscNumber;
                file.Tag.DiscCount = (uint) (track.Album.GetTotalDiscs() ?? 0 );
                file.Tag.Year = (uint) track.Year;
                file.Tag.Copyright = CopyrightText;
                file.Tag.Comment = CopyrightText;
                if (AlbumArtCache.Instance.HasItem(track.Album.CoverUri.ToString()))
                {
                    file.Tag.Pictures = new IPicture[] {CreatePictureFromCache(track.Album.CoverUri.ToString())};
                }
                file.Save();
            }
            string fileName;
            switch (ApplicationSettings.Default.AlbumArtworkSaveFormat)
            {
                case AlbumArtworkSaveFormat.DontSave:
                    break;
                case AlbumArtworkSaveFormat.AsCover:
                    
                    break;
                case AlbumArtworkSaveFormat.AsArtistAlbum:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
