using System;

namespace Athame.PluginAPI.Service
{
    public class Artist
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Uri PictureUrl { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}