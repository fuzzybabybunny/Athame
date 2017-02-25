using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Downloader
{
    public enum DownloadState
    {
        PreProcess,
        DownloadingAlbumArtwork,
        Downloading,
        PostProcess
    }

    public class DownloadEventArgs : EventArgs
    {
        public DownloadState State { get; set; }
        public decimal PercentCompleted { get; set; }
    }

    public interface IDownloader
    {
        event EventHandler<DownloadEventArgs> Progress;
        event EventHandler Done;

        Task DownloadAsyncTask(DownloadTrack track, string destination);

    }
}