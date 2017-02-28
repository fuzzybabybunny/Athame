using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Downloader
{
    public class HttpDownloader : IDownloader, IDisposable
    {
        private readonly WebClient mClient = new WebClient();
        private DownloadState internalState = DownloadState.PreProcess;

        public HttpDownloader()
        {
            mClient.DownloadProgressChanged += OnDownloadProgressChanged;
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            // So we can use the same WebClient for download album artwork
            if (internalState != DownloadState.Downloading) return;
            var percentage = (decimal) downloadProgressChangedEventArgs.BytesReceived /
                               downloadProgressChangedEventArgs.TotalBytesToReceive;
            var eventArgs = new DownloadEventArgs
            {
                State = internalState,
                PercentCompleted = percentage
            };
            Progress?.Invoke(this, eventArgs);
        }

        public event EventHandler<DownloadEventArgs> Progress;
        public event EventHandler Done;
        public async Task DownloadAsyncTask(TrackFile track, string destination)
        {
            var pathWithExtension = $"{destination}.{track.FileType.Extension}";
            await mClient.DownloadFileTaskAsync(track.DownloadUri, pathWithExtension);
            Done?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            mClient?.Dispose();
        }
    }
}
