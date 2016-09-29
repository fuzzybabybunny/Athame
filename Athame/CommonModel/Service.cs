using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athame.CommonModel
{
    public abstract class Service
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
        /// <see cref="RestoreSessionAsync"/> in a try-catch block that will catch a <see cref="NotImplementedException"/>, then fall back to <see cref="RestoreSession"/>.
        /// </summary>
        /// <param name="response">A response returned by <see cref="LoginAsync"/>.</param>
        /// <returns>True on success, otherwise false.</returns>
        public abstract Task<bool> RestoreSessionAsync(AuthenticationResponse response);

        /// <summary>
        /// Restores the user's session. A service should only implement either <see cref="RestoreSession"/> or <see cref="RestoreSessionAsync"/>,
        /// depending on whether they require restoring the session via HTTP or via setting an internal value. To use either method, it is recommended to first try
        /// <see cref="RestoreSessionAsync"/> in a try-catch block that will catch a <see cref="NotImplementedException"/>, then fall back to <see cref="RestoreSession"/>.
        /// </summary>
        /// <param name="response">A response returned by <see cref="LoginAsync"/>.</param>
        /// <returns>True on success, otherwise false.</returns>
        public abstract bool RestoreSession(AuthenticationResponse response);

        /// <summary>
        /// Clears the user's session.
        /// </summary>
        public abstract void ClearSession();

        /// <summary>
        /// Retrieves an album, with tracks.
        /// </summary>
        /// <param name="albumId">The service-specific album identifier.</param>
        /// <returns>An album on success, null otherwise.</returns>
        public abstract Task<Album> GetAlbumWithTracksAsync(string albumId);

        /// <summary>
        /// Retrieves a URI for streaming or downloading a track.
        /// </summary>
        /// <param name="trackId">The service-specific track identifier.</param>
        /// <returns>A <see cref="Uri"/> on success, null otherwise.</returns>
        public abstract Task<Uri> GetTrackStreamUriAsync(string trackId);

        /// <summary>
        /// Parses a public-facing URL of a service, and returns the media type referenced and the identifier.
        /// </summary>
        /// <param name="url">A URL to parse.</param>
        /// <returns>A <see cref="UrlParseResult"/> containing a media type and ID.</returns>
        /// <exception cref="InvalidServiceUrlException">When the URI's host doesn't match the service's <see cref="WebHost"/> property.</exception>
        public abstract UrlParseResult ParseUrl(Uri url);

        /// <summary>
        /// The service's name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The hostname of the service's public website.
        /// </summary>
        public abstract string WebHost { get; }

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
    }
}
