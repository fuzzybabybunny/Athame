using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using OpenTidl;
using OpenTidl.Enums;
using OpenTidl.Methods;
using OpenTidl.Models;
using OpenTidl.Transport;

namespace Athame.TidalApi
{
    public class TidalService : MusicService
    {
        private readonly OpenTidlClient client;
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
            var user = await session.GetUser();
            return new AuthenticationResponse
            {
                Token = session.SessionId,
                UserIdentity = null,
                UserName = String.IsNullOrEmpty(user.FirstName) && String.IsNullOrEmpty(user.LastName) ? 
                            user.Email : $"{user.FirstName} {user.LastName} ({user.Email})"
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
                Id = tidalTrack.Id.ToString(),
                IsDownloadable = tidalTrack.AllowStreaming

            };
            // Only use first artist name and picture for now
            t.Artist = CreateArtist(tidalTrack.Artists, tidalTrack.Artist);
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

        private Artist CreateArtist(ArtistModel[] artists, ArtistModel defaultArtist)
        {
            return new Artist
            {
                Id = defaultArtist.Id.ToString(),
                Name = EnglishArtistNameJoiner.JoinArtistNames((from artist in artists
                                                                where artist.Type == EnglishArtistNameJoiner.ArtistMain
                                                                select artist.Name).ToArray()),
                PictureUrl = new Uri(defaultArtist.Picture)
            };
        }

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
                cmAlbum.Artist = CreateArtist(album.Artists, album.Artist);
            }
            return cmAlbum;
        }

        public override async Task<TrackFile> GetDownloadableTrackAsync(Track track)
        {
            var response = await session.GetTrackOfflineUrl(Int32.Parse(track.Id), settings.StreamQuality);
            var result = new TrackFile {DownloadUri = new Uri(response.Url), Track = track};
            // We can assume the MIME type and bitrate from the **returned** sound quality
            // It is unwise to use the stream quality stored in settings as users with lossless
            // subscriptions will get lossy streams simply because lossless streams are unavailable
            switch (response.SoundQuality)
            {
                case SoundQuality.LOW:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 96 * 1000;
                    break;
                case SoundQuality.HIGH:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 320 * 1000;
                    break;
                case SoundQuality.LOSSLESS:
                    result.FileType = MediaFileTypes.FreeLosslessAudioCodec;
                    // Bitrate doesn't really matter since it's lossless
                    result.BitRate = -1;
                    break;
                case SoundQuality.LOSSLESS_HD:
                    // This seems to be obsolete so I'll just wait and see
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
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

        public override async Task<Album> GetAlbumAsync(string albumId, bool withTracks)
        {
            var tidalAlbum = await client.GetAlbum(Int32.Parse(albumId));
            if (!withTracks)
            {
                return CreateAlbum(tidalAlbum);
            }
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

        public override async Task<Track> GetTrackAsync(string trackId)
        {
            var track = await client.GetTrack(Int32.Parse(trackId));
            var album = await client.GetAlbum(track.Album.Id);
            return CreateTrack(album, track);
        }

        public override string Name => "Tidal";

        public override bool IsAuthenticated => session != null;

        public override AuthenticationMethod AuthenticationMethod => AuthenticationMethod.UsernameAndPassword;

        public override AuthenticationFlow Flow => new AuthenticationFlow
        {
            SignInInformation = "Enter your Tidal username and password:",
            LinksToDisplay = new[]
            {
                new SignInLink{ DisplayName = "Forgot password?", Link = new Uri("https://listen.tidal.com/")}
            }
        };

        public override Control GetSettingsControl()
        {
            return new TidalSettingsControl(settings);
        }

        public override StoredSettings Settings
        {
            get { return settings; }
            set { settings = (TidalServiceSettings)value ?? new TidalServiceSettings(); }
        }

        public override Uri[] BaseUri
            => new[]
            {
                new Uri("http://" + TidalWebDomain), new Uri("https://" + TidalWebDomain),
                new Uri("http://tidal.com"), new Uri("https://tidal.com"),
            };
    }
}
