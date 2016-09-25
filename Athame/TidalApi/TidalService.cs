using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.CommonModel;
using OpenTidl;
using OpenTidl.Enums;
using OpenTidl.Methods;
using OpenTidl.Models;
using OpenTidl.Transport;

namespace Athame.TidalApi
{
    public class TidalService : Service
    {
        private OpenTidlClient client;
        private OpenTidlSession session;
        private TidalServiceSettings settings = new TidalServiceSettings();
        private const string TidalWebDomain = "listen.tidal.com";

        public TidalService()
        {
            client = new OpenTidlClient(ClientConfiguration.Default);
        }

        public override async Task<AuthenticationResponse> LoginAsync(string username, string password)
        {
            try
            {
                session = await client.LoginWithUsername(username, password);
            }
            catch (OpenTidlException)
            {
                return null;
            }
            return new AuthenticationResponse
            {
                Token = session.SessionId,
                UserIdentity = session.CountryCode,
                UserName = username
            };
        }

        /// <summary>
        /// This method is not implemented and will always throw a <see cref="NotImplementedException"/>. Use <see cref="RestoreSession"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
#pragma warning disable 1998
        public override async Task<bool> RestoreSessionAsync(AuthenticationResponse response)
#pragma warning restore 1998
        {
            try
            {
                session = await client.RestoreSession(response.Token);
            }
            catch (OpenTidlException)
            {
                return false;
            }
            return session != null;
        }

        /// <summary>
        /// Sets the session to be used in future request.
        /// </summary>
        /// <param name="response">A login response obtained from <see cref="LoginAsync"/>.</param>
        /// <returns>Always true.</returns>
        public override bool RestoreSession(AuthenticationResponse response)
        {
            throw new NotImplementedException();
            session = new OpenTidlSession(client, new LoginModel());
            return true;
        }

        public override void ClearSession()
        {
            session = null;
            settings.Response = null;
        }

        private Track CreateTrack(TrackModel tidalTrack)
        {
            // Always put main artists in the artist field
            var t = new Track
            {
                DiscNumber = tidalTrack.VolumeNumber,
                TrackNumber = tidalTrack.TrackNumber,
                Title = tidalTrack.Title,
                Artist = EnglishArtistNameJoiner.JoinArtistNames((from artist in tidalTrack.Artists
                    where artist.Type == EnglishArtistNameJoiner.ArtistMain
                    select artist.Name).ToArray()),
                Id = tidalTrack.Id.ToString(),
                FileExtension = settings.StreamQuality == SoundQuality.LOSSLESS || 
                                    settings.StreamQuality == SoundQuality.LOSSLESS_HD
                                    ? ".flac"
                                    : ".m4a"

            };
            // If the featured artists aren't already in the title, append them there
            if (!EnglishArtistNameJoiner.DoesTitleContainArtistString(tidalTrack))
            {
                var nonMainArtists = (from artist in tidalTrack.Artists
                    where artist.Type != EnglishArtistNameJoiner.ArtistMain
                    select artist.Name).ToArray();
                if (nonMainArtists.Length > 0)
                {
                    t.Title += " " + EnglishArtistNameJoiner.JoinFeaturingArtists(nonMainArtists);
                }
            }
            if (tidalTrack.Album != null && tidalTrack.Album.Artists != null)
            {
                t.Album = CreateAlbum(tidalTrack.Album);
            }
            return t;
        }

        private Track CreateTrack(AlbumModel tidalAlbum, TrackModel tidalTrack)
        {
            var t = CreateTrack(tidalTrack);
            if (tidalAlbum.ReleaseDate != null) t.Year = tidalAlbum.ReleaseDate.Value.Year;
            t.Album = CreateAlbum(tidalAlbum);
            return t;
        }

        private Album CreateAlbum(AlbumModel album, List<TrackModel> tracks)
        {
            var a = CreateAlbum(album);
            a.Tracks = new List<Track>(from t in tracks select CreateTrack(album, t));
            return a;
        }

        private const int AlbumArtSize = 1280;
        private const string AlbumArtUrlFormat = "https://resources.tidal.com/images/{0}/{1}x{1}.jpg";
        

        private Album CreateAlbum(AlbumModel album)
        {
            var coverUrl = String.Format(AlbumArtUrlFormat, album.Cover.Replace('-', '/'), AlbumArtSize);
            return new Album
            {
                // Need only main artists
                Artist = EnglishArtistNameJoiner.JoinArtistNames((from artist in album.Artists
                                                                  where artist.Type == EnglishArtistNameJoiner.ArtistMain
                                                                  select artist.Name).ToArray()),
                Id = album.Id.ToString(),
                Title = album.Title,
                CoverUri = new Uri(coverUrl)
            };
        }

        public override async Task<Album> GetAlbumWithTracksAsync(string albumId)
        {
            var tidalAlbum = await client.GetAlbum(Int32.Parse(albumId));
            var tidalTracks = await client.GetAlbumTracks(Int32.Parse(albumId));
            var cmAlbum = CreateAlbum(tidalAlbum);
            var cmTracks = new List<Track>();
            foreach (var track in tidalTracks.Items)
            {
                var cmTrack = CreateTrack(track);
                cmTrack.Album = cmAlbum;
                if (tidalAlbum.ReleaseDate != null) cmTrack.Year = tidalAlbum.ReleaseDate.Value.Year;
                cmTracks.Add(cmTrack);
            }
            cmAlbum.Tracks = cmTracks;
            return cmAlbum;
        }

        public override async Task<Uri> GetTrackStreamUriAsync(string trackId)
        {
            return new Uri((await session.GetTrackOfflineUrl(Int32.Parse(trackId), settings.StreamQuality)).Url);
        }

        public override UrlParseResult ParseUrl(Uri url)
        {
            if (url.Host != TidalWebDomain)
            {
                throw new InvalidServiceUrlException("Not a Tidal URL.");
            }
            var pathParts = url.LocalPath.Split('/');
            var ctype = pathParts[1];
            var id = pathParts[2];
            var result = new UrlParseResult {Id = id, Type = UrlContentType.Unknown};
            switch (ctype)
            {
                case "album":
                    result.Type = UrlContentType.Album;
                    break;

                case "track":
                    result.Type = UrlContentType.Track;
                    break;

                case "artist":
                    result.Type = UrlContentType.Artist;
                    break;

                case "playlist":
                    result.Type = UrlContentType.Playlist;
                    break;

                default:
                    result.Type = UrlContentType.Unknown;
                    break;
            }
            return result;
        }

        public override string Name
        {
            get { return "Tidal"; }
        }

        public override string WebHost
        {
            get { return TidalWebDomain; }
        }

        public override bool IsAuthenticated
        {
            get { return session != null; }
        }

        public override Control GetSettingsControl()
        {
            return new TidalSettingsControl(settings);
        }

        public override StoredSettings Settings
        {
            get { return settings; }
            set { settings = (TidalServiceSettings)value ?? new TidalServiceSettings(); }
        }
    }
}
