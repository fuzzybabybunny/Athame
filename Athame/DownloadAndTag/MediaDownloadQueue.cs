using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using Athame.Utils;

namespace Athame.DownloadAndTag
{
    public class CollectionDownloadEventArgs : EventArgs
    {
        public EnqueuedCollection Collection { get; set; }
    }

    public class TrackDownloadEventArgs : DownloadEventArgs
    {
        public TrackFile TrackFile { get; set; }

        public decimal TotalProgress
        {
            get {
                if (TotalItems == 0)
                {
                    return 0;
                }
                return (CurrentItemIndex + PercentCompleted) / TotalItems;
            }
        }

        public int TotalItems { get; set; }

        public int CurrentItemIndex { get; set; }

        internal void Update(DownloadEventArgs eventArgs)
        {
            PercentCompleted = eventArgs.PercentCompleted;
            State = eventArgs.State;
        }
    }

    public class MediaDownloadQueue
    {

        private class SingleTrackCollection : IMediaCollection
        {
            public SingleTrackCollection(Track t)
            {
                Tracks = new[] { t };
                Title = t.Title;
            }

            public IEnumerable<Track> Tracks { get; set; }
            public string Title { get; set; }
        }

        private readonly WebClient mAlbumArtworkClient = new WebClient();
        private readonly TrackTagger mTagger = new TrackTagger();
        private readonly List<EnqueuedCollection> mList = new List<EnqueuedCollection>();

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void Enqueue(MusicService service, Track track, string pathFormat)
        {
            mList.Add(new EnqueuedCollection
            {
                Collection = new SingleTrackCollection(track),
                Service = service,
                PathFormat = pathFormat
            });
        }

        public void EnqueueCollection(MusicService service, IMediaCollection collection, string pathFormat)
        {
            mList.Add(new EnqueuedCollection
            {
                Collection = collection,
                Service = service,
                PathFormat = pathFormat
            });
        }

        #region Events
        /// <summary>
        /// Raised before a media collection is downloaded.
        /// </summary>
        public event EventHandler<CollectionDownloadEventArgs> CollectionDequeued;

        protected void OnCollectionDequeued(CollectionDownloadEventArgs e)
        {
            CollectionDequeued?.Invoke(this, e);
        }

        /// <summary>
        /// Raised before a track is downloaded.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDequeued;

        protected void OnTrackDequeued(TrackDownloadEventArgs e)
        {
            TrackDequeued?.Invoke(this, e);
        }

        /// <summary>
        /// Raised after a track is downloaded.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;

        protected void OnTrackDownloadCompleted(TrackDownloadEventArgs e)
        {

            TrackDownloadCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Raised when a track's download progress changes.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadProgress;

        protected void OnTrackDownloadProgress(TrackDownloadEventArgs e)
        {
            TrackDownloadProgress?.Invoke(this, e);
        }
#endregion

        public async Task StartDownloadAsync()
        {
            var queueView = new Queue<EnqueuedCollection>(mList);
            while (queueView.Count > 0)
            {
                var currentItem = queueView.Dequeue();
                OnCollectionDequeued(new CollectionDownloadEventArgs
                {
                    Collection = currentItem
                });
                await DownloadCollectionAsync(currentItem);
            }
        }

        private async Task DownloadCollectionAsync(EnqueuedCollection collection)
        {
            // If Tracks is an ICollection, Count() will just return ICollection.Count property,
            // otherwise it'll enumerate, hence why we only want to call it once
            var tracksCollectionLength = collection.Collection.Tracks.Count();
            var tracksQueue = new Queue<Track>(collection.Collection.Tracks);
            while (tracksQueue.Count > 0)
            {
                var currentItem = tracksQueue.Dequeue();
                var eventArgs = new TrackDownloadEventArgs
                {
                    CurrentItemIndex = tracksCollectionLength - tracksQueue.Count,
                    PercentCompleted = 0M,
                    State = DownloadState.PreProcess,
                    TotalItems = tracksCollectionLength,
                    TrackFile = null
                };
                OnTrackDequeued(eventArgs);
                // Download album artwork if it's not cached
                if (currentItem.Album != null && !AlbumArtCache.Instance.HasItem(currentItem.Album.CoverUri.ToString()))
                {
                    eventArgs.State = DownloadState.DownloadingAlbumArtwork;
                    OnTrackDownloadProgress(eventArgs);
                    await AlbumArtCache.Instance.AddByDownload(currentItem.Album.CoverUri.ToString());
                }
                eventArgs.TrackFile = await collection.Service.GetDownloadableTrackAsync(currentItem);
                var downloader = collection.Service.GetDownloader(eventArgs.TrackFile);
                downloader.Progress += (sender, args) =>
                {
                    eventArgs.Update(args);
                    OnTrackDownloadProgress(eventArgs);
                };
                downloader.Done += (sender, args) =>
                {
                    OnTrackDownloadCompleted(eventArgs);
                };
                await downloader.DownloadAsyncTask(eventArgs.TrackFile,
                    eventArgs.TrackFile.GetPath(collection.PathFormat));
                // Attempt to dispose the downloader, since the most probable case will be that it will
                // implement IDisposable if it uses sockets
                var disposableDownloader = downloader as IDisposable;
                disposableDownloader?.Dispose();
                // Write the tag

            }
        }
    }
}
