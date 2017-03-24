using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;

namespace Athame.DownloadAndTag
{
    internal class AlbumArtCache : IDisposable
    {
        public static readonly AlbumArtCache Instance = new AlbumArtCache();

        private readonly Dictionary<string, AlbumArtFile> items = new Dictionary<string, AlbumArtFile>();
        private readonly WebClient mClient = new WebClient();

        public async Task AddByDownload(string url)
        {
            var data = await mClient.DownloadDataTaskAsync(url);
            var contentMimeType = mClient.ResponseHeaders[HttpResponseHeader.ContentType];

            items.Add(url, new AlbumArtFile
            {
                Data = data,
                DownloadUri = new Uri(url),
                FileType = MediaFileTypes.ByMimeType(contentMimeType)
            });
        }

        public void Add(AlbumArtFile file)
        {
            items[file.DownloadUri.ToString()] = file;
        }

        public bool HasItem(string url)
        {
            return items.ContainsKey(url);
        }

        public AlbumArtFile Get(string url)
        {
            return items[url];
        }

        public void WriteToFile(string url, string filePath)
        {
            var item = items[url];
            File.WriteAllBytes(item.FileType.Append(filePath), item.Data);
        }

        public void Dispose()
        {
            mClient?.Dispose();
        }
    }
}
