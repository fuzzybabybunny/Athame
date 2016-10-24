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
        /// Sets the session to be used in future request.
        /// </summary>
        /// <param name="response">A login response obtained from <see cref="LoginAsync"/>.</param>
        /// <returns>True if it succeeded, otherwise false.</returns>
        public override async Task<bool> RestoreSessionAsync(AuthenticationResponse response)
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
        /// This method is not implemented and will always throw a <see cref="NotImplementedException"/>. Use <see cref="RestoreSessionAsync"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public override bool RestoreSession(AuthenticationResponse response)
        {
            throw new NotImplementedException();
        }

        public override void ClearSession()
        {
            session = null;
            settings.Response = null;
        }

        private Track CreateTrack(TrackModel tidalTrack)
        {
            const string albumVersion = "Album Version";
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
                IsDownloadable = tidalTrack.AllowStreaming,
                FileExtension = settings.StreamQuality == SoundQuality.LOSSLESS || 
                                    settings.StreamQuality == SoundQuality.LOSSLESS_HD
                                    ? ".flac"
                                    : ".m4a"

            };
            if (!String.IsNullOrEmpty(tidalTrack.Version))
            {
                if (settings.AppendVersionToTrackTitle)
                {
                    if (settings.DontAppendAlbumVersion)
                    {
                        if (!tidalTrack.Version.Contains(albumVersion))
                        {
                            t.Title += " (" + tidalTrack.Version + ")";
                        }
                    }
                    else
                    {
                        t.Title += " (" + tidalTrack.Version + ")";
                    }
                }
            }
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
            t.Album = CreateAlbum(tidalTrack.Album);
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
            var cmAlbum = new Album
            {
                Id = album.Id.ToString(),
                Title = album.Title,
                CoverUri = new Uri(coverUrl)
            };
            // On most calls the Album returned is a "lite" version, with only the properties above
            // available.
            if (album.Artist != null)
            {
                // Need only main artists
                cmAlbum.Artist = EnglishArtistNameJoiner.JoinArtistNames((from artist in album.Artists
                    where artist.Type == EnglishArtistNameJoiner.ArtistMain
                    select artist.Name).ToArray());
            }
            return cmAlbum;
        }

        public override async Task<Album> GetAlbumWithTracksAsync(string albumId)
        {
//            try
//            {
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
//            }
//            catch (OpenTidlException ex)
//            {
//                if (ex.OpenTidlError.Status == 404)
//                {
//                    throw new ResourceNotFoundException(String.Format("The album {0} was not found on Tidal.", albumId));
//                }
//                throw;
//            }
        }

        public override async Task<Uri> GetTrackStreamUriAsync(string trackId)
        {
            return new Uri((await session.GetTrackOfflineUrl(Int32.Parse(trackId), settings.StreamQuality)).Url);
        }

        public override async Task<Playlist> GetPlaylistAsync(string playlistId)
        {
            var playlist = await session.GetPlaylist(playlistId);
            var tracks = await session.GetPlaylistTracks(playlistId);
            return new Playlist
            {
                Title = playlist.Title,
                Tracks = (from t in tracks.Items select CreateTrack(t)).ToList()
            };
        }

        public override UrlParseResult ParseUrl(Uri url)
        {
            if (url.Host != TidalWebDomain)
            {
                return null;
            }
            var pathParts = url.LocalPath.Split('/');
            if (pathParts.Length <= 2) return null;
            var ctype = pathParts[1];
            var id = pathParts[2];
            var result = new UrlParseResult {Id = id, Type = MediaType.Unknown, OriginalUri = url};
            switch (ctype)
            {
                case "album":
                    result.Type = MediaType.Album;
                    break;

                case "track":
                    result.Type = MediaType.Track;
                    break;

                case "artist":
                    result.Type = MediaType.Artist;
                    break;

                case "playlist":
                    result.Type = MediaType.Playlist;
                    break;

                default:
                    result.Type = MediaType.Unknown;
                    break;
            }
            return result;
        }

        public override Task<SearchResult> SearchAsync(string searchText, MediaType typesToRetrieve)
        {
            throw new NotImplementedException();
        }

        public override Task<Album> GetAlbumAsync(string albumId, bool withTracks)
        {
            throw new NotImplementedException();
        }

        public override Task<Track> GetTrackAsync(string trackId)
        {
            throw new NotImplementedException();
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

        public override AuthenticationMethod AuthenticationMethod
        {
            get
            {
                return AuthenticationMethod.UsernameAndPassword;
            }
        }

        public override AuthenticationFlow Flow
        {
            get
            {
                return new AuthenticationFlow
                {
                    SignInInformation = "Enter your Tidal username and password:",
                    LinksToDisplay = new Dictionary<string, string>
                    {
                        {"Forgot password?", "https://listen.tidal.com/"}
                    }
                };
            }
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
