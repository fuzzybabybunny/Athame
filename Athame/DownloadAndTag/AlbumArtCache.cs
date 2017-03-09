using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Athame.DownloadAndTag
{
    internal class AlbumArtCache : IDisposable
    {
        public static readonly AlbumArtCache Instance = new AlbumArtCache();

        private readonly Dictionary<string, byte[]> items = new Dictionary<string, byte[]>();
        private readonly WebClient mClient = new WebClient();

        public async Task AddByDownload(string url)
        {
            var data = await mClient.DownloadDataTaskAsync(url);
            var contentMimeType = mClient.ResponseHeaders[HttpResponseHeader.ContentType];

            items.Add(url, data);
        }

        public void AddByFilename(string url, string filePath)
        {
            items.Add(url, File.ReadAllBytes(filePath));
        }

        public void Add(string url, byte[] data)
        {
            items.Add(url, data);
        }

        public bool HasItem(string url)
        {
            return items.ContainsKey(url);
        }

        public byte[] Get(string url)
        {
            return items[url];
        }

        public void WriteToFile(string url, string filePath)
        {
            File.WriteAllBytes(filePath, items[url]);
        }

        public void Dispose()
        {
            mClient?.Dispose();
        }
    }
}
