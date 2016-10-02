using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Athame.CommonModel;
using Athame.InternalModel;

namespace Athame.DownloadAndTag
{
    public class DownloadProgressEventArgs : EventArgs
    {
        public int CurrentItemIndex { get; internal set; }
        public DownloadableTrack CurrentTrack { get; set; }
        public int TotalItems { get; internal set; }
        public int OverallCompletionPercentage
        {
            get 
            { 
                var currentPercent = CurrentProgress != null ? 
                    CurrentProgress.ProgressPercentage : 
                    100;
                var overallPercentage = (int)(((double)CurrentItemIndex / (double)TotalItems) * (double)100) + currentPercent / TotalItems;
                return overallPercentage > 100 ? 100 : overallPercentage;
            }
        }
        public DownloadProgressChangedEventArgs CurrentProgress { get; internal set; }
    }

    public class TrackDownloader : IDisposable
    {
        private readonly WebClient mClient = new WebClient();
        private readonly List<DownloadableTrack> tracks;
        private readonly string pathFormat;
        private readonly Service service;

        private int currentTrack;
        
        public TrackDownloader(Service service, List<DownloadableTrack> tracks, string pathFormat)
        {
            this.tracks = tracks;
            this.pathFormat = pathFormat;
            this.service = service;

            mClient.DownloadProgressChanged += (sender, args) =>
            {
                OnItemProgressChanged(CreateArgs(args));
            };
        }

        #region Events
        /// <summary>
        /// Raised when the item's total (artwork + stream URL retrieval + track download) progress changes.
        /// </summary>
        public event EventHandler<DownloadProgressEventArgs> ItemProgressChanged;

        /// <summary>
        /// Raised when the item has fully completed downloading.
        /// </summary>
        public event EventHandler<DownloadProgressEventArgs> ItemDownloadCompleted;

        protected void OnItemProgressChanged(DownloadProgressEventArgs e)
        {
            if (ItemProgressChanged != null)
            {
                ItemProgressChanged(this, e);
            }
        }

        protected void OnItemDownloadCompleted(DownloadProgressEventArgs e)
        {
            if (ItemDownloadCompleted != null)
            {
                ItemDownloadCompleted(this, e);
            }
        }
        #endregion

        private DownloadProgressEventArgs CreateArgs(DownloadProgressChangedEventArgs args)
        {
            
            return new DownloadProgressEventArgs
            {
                CurrentProgress = args,
                CurrentItemIndex = currentTrack,
                CurrentTrack = tracks[currentTrack],
                TotalItems = tracks.Count
            };
        }

        public async Task DownloadAsync()
        {
            currentTrack = 0;

            foreach (var track in tracks)
            {
                track.SetPathFromFormat(pathFormat);

                // Ensure directory structure exists
                var parentDirectory = Path.GetDirectoryName(track.Path) ?? "";
                Directory.CreateDirectory(parentDirectory);

                // Download artwork if it doesn't exist
                if (!File.Exists(track.ArtworkPath))
                {
                    track.State = TrackState.DownloadingArtwork;
                    await mClient.DownloadFileTaskAsync(track.CommonTrack.Album.CoverUri, track.ArtworkPath);
                }

                // Get stream URL
                track.State = TrackState.DownloadingTrack;
                var streamUrl = await service.GetTrackStreamUriAsync(track.CommonTrack.Id);
                
                // Download completely
                await mClient.DownloadFileTaskAsync(streamUrl, track.Path);

                // Done!
                track.State = TrackState.Complete;
                OnItemDownloadCompleted(CreateArgs(null));
                currentTrack++;
            }
        }

        public void Dispose()
        {
            mClient.Dispose();
        }
    }
}
