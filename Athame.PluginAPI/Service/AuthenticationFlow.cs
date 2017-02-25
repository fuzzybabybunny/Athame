using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// How the service authenticates itself.
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// The service does not require authentication.
        /// </summary>
        None,
        /// <summary>
        /// The service authenticates through username and password.
        /// </summary>
        UsernameAndPassword,
        /// <summary>
        /// The service provides custom logic (such as showing a custom window or signing in through an OAuth provider) in order to authenticate.
        /// When authentication is requested, <see cref="MusicService.DoCustomAuthentication"/> is first called. If it is not implemented, then <see cref="MusicService.DoCustomAuthenticationAsync"/>
        /// is called. You must implement one or the other to use this authentication type.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Provides information on how the authentication process should proceed.
    /// </summary>
    public class AuthenticationFlow
    {
        /// <summary>
        /// A string of text displayed to the user above the username and password fields of the sign-in credentials dialog.
        /// </summary>
        public string SignInInformation { get; set; }

        /// <summary>
        /// A dictionary of links displayed below the sign in information string. The key will be displayed as the link text, and the value
        /// will be a URL which is opened when the link is clicked.
        /// </summary>
        public IEnumerable<SignInLink> LinksToDisplay { get; set; }
    }
}
