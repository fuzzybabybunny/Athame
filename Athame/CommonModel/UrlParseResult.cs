using System;

namespace Athame.CommonModel
{
    public enum UrlContentType
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
        public UrlContentType Type { get; set; }
        public string Id { get; set; }
    }
}
