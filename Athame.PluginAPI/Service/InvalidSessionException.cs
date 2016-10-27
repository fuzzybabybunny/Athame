using System;

namespace Athame.PluginAPI.Service
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
            
        }
    }
}
