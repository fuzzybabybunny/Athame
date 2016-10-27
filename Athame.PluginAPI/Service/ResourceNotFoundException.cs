using System;

namespace Athame.PluginAPI.Service
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
            
        }
    }
}
