using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// How the service authenticates itself.
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// The service does not require authentication
        /// </summary>
        None,
        /// <summary>
        /// The service authenticates through username and password
        /// </summary>
        UsernameAndPassword,
        /// <summary>
        /// Not yet implemented. The service provides custom logic (such as showing a custom window or signing in through an OAuth provider) in order to authenticate
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
        public IReadOnlyDictionary<string, string> LinksToDisplay { get; set; }
    }
}
