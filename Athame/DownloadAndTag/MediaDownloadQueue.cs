using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;

namespace Athame.DownloadAndTag
{
    public class TrackDownloadEventArgs : DownloadEventArgs
    {
        public TrackFile TrackFile { get; set; }
        public EnqueuedTrack Track { get; set; }
        public decimal TotalProgress { get; set; }
    }

    public class MediaDownloadQueue
    {
        private readonly List<EnqueuedTrack> mList = new List<EnqueuedTrack>();

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void Enqueue(MusicService service, Track track)
        {
            mList.Add(new EnqueuedTrack { Service = service, Track = track});
        }

        public void EnqueueCollection(MusicService service, IMediaCollection collection)
        {
            var enqueuedTracks = from item in collection.Tracks
                select new EnqueuedTrack {Service = service, Track = item};
            mList.AddRange(enqueuedTracks);
        }

        public event EventHandler<TrackDownloadEventArgs> TrackDequeued;

        protected void OnTrackDequeued(TrackDownloadEventArgs e)
        {
            TrackDequeued?.Invoke(this, e);
        }
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;

        protected void OnTrackDownloadCompleted(TrackDownloadEventArgs e)
        {
            TrackDownloadCompleted?.Invoke(this, e);
        }
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadProgress;

        protected void OnTrackDownloadProgress(TrackDownloadEventArgs e)
        {
            TrackDownloadProgress?.Invoke(this, e);
        }

        public async Task StartDownloadAsync()
        {
            var queueView = new Queue<EnqueuedTrack>(mList);
            while (queueView.Count > 0)
            {
                var currentItem = queueView.Dequeue();
                var eventArgs = new TrackDownloadEventArgs
                {
                    PercentCompleted = 0,
                    State = DownloadState.PreProcess,
                    TotalProgress = (((decimal)mList.Count - queueView.Count) / mList.Count) * 100,
                    Track = currentItem,
                    TrackFile = null
                };
                OnTrackDequeued(eventArgs);

            }
        }
    }
}
