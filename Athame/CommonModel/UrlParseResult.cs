using System;

namespace Athame.CommonModel
{
    public enum MediaType
    {
        Unknown,
        Album,
        Track,
        Playlist,
        Artist
    }

    public class InvalidServiceUrlException : Exception
    {
        public InvalidServiceUrlException(string message) : base(message)
        {
            
        }
    }

    public class UrlParseResult
    {
        public Uri OriginalUri { get; set; }
        public MediaType Type { get; set; }
        public string Id { get; set; }
    }
}
