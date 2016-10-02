using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.CommonModel;
using GoogleMusicApi.Common;

namespace Athame.PlayMusicApi
{
    public class PlayMusicService : Service
    {
        private const string GooglePlayHost = "play.google.com";
        private MobileClient client = new MobileClient();
        private PlayMusicServiceSettings settings = new PlayMusicServiceSettings();

        public override async Task<AuthenticationResponse> LoginAsync(string username, string password)
        {
            await client.LoginAsync(username, password);
            return new AuthenticationResponse
            {
                Token = client.Session.MasterToken,
                UserIdentity = username,
                UserName = client.Session.FirstName + " " + client.Session.LastName + " (" + username + ")"
            };
        }

        public override async Task<bool> RestoreSessionAsync(AuthenticationResponse response)
        {
            return await client.LoginWithToken(response.UserIdentity, response.Token);
        }

        public override bool RestoreSession(AuthenticationResponse response)
        {
            throw new NotImplementedException();
        }

        public override void ClearSession()
        {
            // reinit client to clear stored session
            client = new MobileClient();
            settings.Response = null;
        }

        private Track CreateTrack(GoogleMusicApi.Structure.Track gpmTrack)
        {
            return new Track
            {
                Artist = gpmTrack.Artist,
                DiscNumber = gpmTrack.DiscNumber,
                Genre = gpmTrack.Genre,
                Title = gpmTrack.Title,
                Year = gpmTrack.Year,
                TrackNumber = gpmTrack.TrackNumber,
                Id = gpmTrack.StoreId,
                FileExtension = ".mp3"
            };
        }

        private Album CreateAlbum(GoogleMusicApi.Structure.Album gpmAlbum)
        {
            var a = new Album
            {
                Artist = gpmAlbum.AlbumArtist,
                CoverUri = new Uri(gpmAlbum.AlbumArtRef),
                Title = gpmAlbum.Name,
                Tracks = new List<Track>()
            };
            if (gpmAlbum.Tracks != null)
            {
                foreach (var track in gpmAlbum.Tracks)
                {
                    var cmTrack = CreateTrack(track);
                    cmTrack.Album = a;
                    a.Tracks.Add(cmTrack);
                }
                
            }
            return a;
        }

        private Album CreateAlbum(GoogleMusicApi.Structure.Album album, List<GoogleMusicApi.Structure.Track> tracks)
        {
            var a = CreateAlbum(album);
            a.Tracks = new List<Track>(from t in tracks select CreateTrack(t));
            return a;
        }

        public override async Task<Album> GetAlbumWithTracksAsync(string albumId)
        {
            // Album should always have tracks
            var album = await client.GetAlbumAsync(albumId, includeDescription: false);
            return CreateAlbum(album);
        }

        public override async Task<Uri> GetTrackStreamUriAsync(string trackId)
        {
            // Only property we need to set is Track.StoreId (see Google.Music/GoogleMusicApi.UWP/Requests/Data/StreamUrlGetRequest.cs:32)
            return await client.GetStreamUrlAsync(new GoogleMusicApi.Structure.Track {StoreId = trackId});
        }

        public override UrlParseResult ParseUrl(Uri url)
        {
            if (url.Host != GooglePlayHost)
            {
                throw new InvalidServiceUrlException("Not a Google Play Music URL.");
            }
            var hashParts = url.Fragment.Split('/');
            var type = hashParts[1];
            var id = hashParts[2];
            var result = new UrlParseResult {Id = id, Type = MediaType.Unknown, OriginalUri = url};
            switch (type)
            {
                case "album":
                    result.Type = MediaType.Album;
                    break;
                
                case "artist":
                    result.Type = MediaType.Artist;
                    break;

                    // Will auto-playlists actually be interchangeable with user-generated playlists?
                case "pl":
                case "ap":
                    result.Type = MediaType.Playlist;
                    break;

                default:
                    result.Type = MediaType.Unknown;
                    break;
            }
            return result;
        }

        public override string Name
        {
            get { return "Google Play Music"; }
        }

        public override string WebHost
        {
            get { return GooglePlayHost; }
        }

        public override bool IsAuthenticated
        {
            get { return client.Session != null && client.Session.IsAuthenticated; }
        }

        public override Control GetSettingsControl()
        {
            return new PlayMusicSettingsControl(settings);
        }

        public override StoredSettings Settings
        {
            get
            {
                return settings;
            }
            set { settings = (PlayMusicServiceSettings)value ?? new PlayMusicServiceSettings(); }
        }

        public override AuthenticationMethod AuthenticationMethod
        {
            get { return AuthenticationMethod.UsernameAndPassword;}
        }

        public override AuthenticationFlow Flow
        {
            get
            {
                return new AuthenticationFlow
                {
                    SignInInformation =
                        "Enter your Google account email and password. If you use two-factor authentication, you must set an app password:",
                    LinksToDisplay = new Dictionary<string, string>
                    {
                        {"Set an app password", "https://security.google.com/settings/security/apppasswords"},
                        {"Forgot your password?", "https://accounts.google.com/signin/recovery"}
                    }
                };
            }
        }
    }
}
