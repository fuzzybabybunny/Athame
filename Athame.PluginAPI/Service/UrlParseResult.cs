using System;

namespace Athame.PluginAPI.Service
{
    public class UrlParseResult
    {
        public Uri OriginalUri { get; set; }
        public MediaType Type { get; set; }
        public string Id { get; set; }
    }
}
