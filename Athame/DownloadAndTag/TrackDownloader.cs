using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Athame.CommonModel;

namespace Athame.DownloadAndTag
{
    public enum ProgressStage
    {
        Artwork,
        DownloadUrl,
        Track
    }

    public class DownloadProgressEventArgs : EventArgs
    {
        public ProgressStage Stage { get; set; }
        public int CurrentItem { get; internal set; }
        public int TotalItems { get; internal set; }
        public string DestinationPath { get; internal set; }
        public int OverallCompletionPercentage
        {
            get 
            { 
                var currentPercent = CurrentProgress != null ? 
                    CurrentProgress.ProgressPercentage : 
                    100;
                var overallPercentage = (int)(((double)CurrentItem / (double)TotalItems) * (double)100) + currentPercent / TotalItems;
                return overallPercentage > 100 ? 100 : overallPercentage;
            }
        }

        public string CoverArtPath { get; internal set; }
        public DownloadProgressChangedEventArgs CurrentProgress { get; internal set; }
    }

    public class TrackDownloader : IDisposable
    {
        private const string ArtworkName = "cover.jpg";

        private readonly WebClient mClient = new WebClient();
        private readonly List<Track> tracks;
        private readonly string pathFormat;
        private readonly Service service;

        private int currentTrack;
        private string currentDestination;
        private ProgressStage stage;
        
        public TrackDownloader(Service service, List<Track> tracks, string pathFormat)
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
            var coverArtPath = Path.Combine(Path.GetDirectoryName(currentDestination) ?? "", ArtworkName);
            return new DownloadProgressEventArgs
            {
                CurrentProgress = args,
                CurrentItem = currentTrack,
                TotalItems = tracks.Count,
                DestinationPath = currentDestination,
                Stage = stage,
                CoverArtPath = coverArtPath
            };
        }

        private string PathFormat(Track track)
        {
            return StringObjectFormatter.Format(pathFormat, track, o => PathHelpers.CleanFilename(o.ToString()));
        }

        public async Task DownloadAsync()
        {
            currentTrack = 0;
            currentDestination = String.Empty;
            stage = ProgressStage.Track;

            foreach (var track in tracks)
            {
                currentDestination = PathFormat(track) + track.FileExtension;

                // Ensure directory structure exists
                var parentDirectory = Path.GetDirectoryName(currentDestination) ?? "";
                Directory.CreateDirectory(parentDirectory);

                var artworkLoc = Path.Combine(parentDirectory, ArtworkName);
                // Download artwork if it doesn't exist
                if (!File.Exists(artworkLoc))
                {
                    stage = ProgressStage.Artwork;
                    await mClient.DownloadFileTaskAsync(track.Album.CoverUri, artworkLoc);
                }

                // Get stream URL
                stage = ProgressStage.DownloadUrl;
                var streamUrl = await service.GetTrackStreamUriAsync(track.Id);
                
                // Download completely
                stage = ProgressStage.Track;
                await mClient.DownloadFileTaskAsync(streamUrl, currentDestination);

                // Done!
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
