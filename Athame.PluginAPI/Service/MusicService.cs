using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.PluginAPI.Downloader;

namespace Athame.PluginAPI.Service
{
    public abstract class MusicService
    {
        /// <summary>
        /// Authenticates for the first time against the service.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>An <see cref="AuthenticationResponse"/> containing service-specific data.</returns>
        public abstract Task<AuthenticationResponse> LoginAsync(string username, string password);

        /// <summary>
        /// Restores the user's session via HTTP request. A service should only implement either <see cref="RestoreSession"/> or <see cref="RestoreSessionAsync"/>,
        /// depending on whether they require restoring the session via HTTP or via setting an internal value. To use either method, it is recommended to first try
        /// <see cref="RestoreSessionAsync"/> in a try-catch block that will catch a <see cref="System.NotImplementedException"/>, then fall back to <see cref="RestoreSession"/>.
        /// </summary>
        /// <param name="response">A response returned by <see cref="LoginAsync"/>.</param>
        /// <returns>True on success, otherwise false.</returns>
        public abstract Task<bool> RestoreSessionAsync(AuthenticationResponse response);

        /// <summary>
        /// Restores the user's session. A service should only implement either <see cref="RestoreSession"/> or <see cref="RestoreSessionAsync"/>,
        /// depending on whether they require restoring the session via HTTP or via setting an internal value. To use either method, it is recommended to first try
        /// <see cref="RestoreSessionAsync"/> in a try-catch block that will catch a <see cref="System.NotImplementedException"/>, then fall back to <see cref="RestoreSession"/>.
        /// </summary>
        /// <param name="response">A response returned by <see cref="LoginAsync"/>.</param>
        /// <returns>True on success, otherwise false.</returns>
        public abstract bool RestoreSession(AuthenticationResponse response);

        /// <summary>
        /// Clears the user's session.
        /// </summary>
        public abstract void ClearSession();

        /// <summary>
        /// Retrieves a track's downloadable form.
        /// </summary>
        /// <param name="track">The track to download.</param>
        /// <returns>A <see cref="TrackFile"/> containing file metadata and the URI of the track.</returns>
        public abstract Task<TrackFile> GetDownloadableTrackAsync(Track track);

        /// <summary>
        /// Retrieves a playlist. Note that it is up to the implementation to differentiate
        /// between different playlist types, if the music service specifies them.
        /// </summary>
        /// <param name="playlistId">The playlist ID to retrieve.</param>
        /// <returns>A playlist on success, null otherwise.</returns>
        public abstract Task<Playlist> GetPlaylistAsync(string playlistId);

        /// <summary>
        /// Parses a public-facing URL of a service, and returns the media type referenced and the identifier.
        /// </summary>
        /// <param name="url">A URL to parse.</param>
        /// <returns>A <see cref="UrlParseResult"/> containing a media type and ID.</returns>
        public abstract UrlParseResult ParseUrl(Uri url);

        /// <summary>
        /// Performs a text search and retrieves the results -- see <see cref="SearchResult"/> for what is returned.
        /// </summary>
        /// <param name="searchText">The text to search</param>
        /// <param name="typesToRetrieve">Which media to search for. This can be ignored for services which return all types regardless.</param>
        /// <returns>A <see cref="SearchResult"/> containing top tracks, albums, or playlists.</returns>
        public abstract Task<SearchResult> SearchAsync(string searchText, MediaType typesToRetrieve);

        /// <summary>
        /// Retrieves an album.
        /// </summary>
        /// <param name="albumId">The album's identifier.</param>
        /// <param name="withTracks">Whether to return tracks or not. On some services, this may involve an extra API call. 
        /// Implementations are also allowed to return an object with tracks even if this is false.</param>
        /// <returns>An album, with or without tracks.</returns>
        public abstract Task<Album> GetAlbumAsync(string albumId, bool withTracks);

        /// <summary>
        /// Retrieves the metadata for a single track.
        /// </summary>
        /// <param name="trackId">The track's identifier.</param>
        /// <returns>A track.</returns>
        public abstract Task<Track> GetTrackAsync(string trackId);

        /// <summary>
        /// The service's name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// If a user is authenticated with the service.
        /// </summary>
        public abstract bool IsAuthenticated { get; }

        /// <summary>
        /// The method the service uses to authenticate a user.
        /// </summary>
        public abstract AuthenticationMethod AuthenticationMethod { get; }

        /// <summary>
        /// How the service should present information to the user.
        /// </summary>
        public abstract AuthenticationFlow Flow { get; }

        /// <summary>
        /// Returns a settings control to display in the settings form. Do not cache this in your implementation, as it is always disposed
        /// when the settings form closes.
        /// </summary>
        /// <returns>A settings control to display.</returns>
        public abstract Control GetSettingsControl();

        /// <summary>
        /// An object that holds persistent settings. Settings are deserialized from storage when the service is first initialized and 
        /// serialized when the user clicks "Save" on the settings form and when the application closes. Implementations should provide a
        /// "default" settings instance when there are no persisted settings available.
        /// </summary>
        public abstract StoredSettings Settings { get; set; }

        /// <summary>
        /// The base URI of the service. Entered URIs are compared on the Scheme and Host properties of each base URI, and if they match,
        /// <see cref="ParseUrl"/> is called.
        /// </summary>
        public abstract Uri[] BaseUri { get; }

        /// <summary>
        /// Performs custom authentication. This is only called if the <see cref="AuthenticationMethod"/> is <see cref="AuthenticationMethod.Custom"/>.
        /// You must implement this if you are using custom authentication.
        /// <seealso cref="AuthenticationMethod"/>
        /// </summary>
        /// <param name="parent">The parent form. May be null.</param>
        /// <returns>True if the user has authenticated successfully.</returns>
        public virtual bool DoCustomAuthentication(Form parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs custom authentication asynchronously. This is only called if the <see cref="AuthenticationMethod"/> is <see cref="AuthenticationMethod.Custom"/>.
        /// <seealso cref="DoCustomAuthentication"/>
        /// </summary>
        /// <param name="parent">The parent form. May be null.</param>
        /// <returns>True if the user has authenticated successfully.</returns>
        public virtual Task<bool> DoCustomAuthenticationAsync(Form parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a downloader for this service. The default implementation is <see cref="HttpDownloader"/>,
        /// but this method may be overridden with a custom downloader that implements <see cref="IDownloader"/>.
        /// </summary>
        /// <returns>A new concrete implementation of <see cref="IDownloader"/>.</returns>
        public virtual IDownloader GetDownloader(TrackFile t)
        {
            return new HttpDownloader();
        }
    }
}
