using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public int CurrentCollectionIndex { get; set; }

        public int TotalNumberOfCollections { get; set; }
    }

    public class TrackDownloadEventArgs : DownloadEventArgs
    {
        public TrackFile TrackFile { get; set; }

        public decimal TotalProgress
        {
            get
            {
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

    /// <summary>
    /// Defines what to skip to next when an exception is encountered.
    /// </summary>
    public enum ExceptionSkip
    {
        /// <summary>
        /// The downloader should advance to the next item.
        /// </summary>
        Item,
        /// <summary>
        /// The downloader should advance to the next collection.
        /// </summary>
        Collection,
        /// <summary>
        /// The downloader should stop and return immediately.
        /// </summary>
        Fail
    }

    /// <summary>
    /// Event args used when an exception occurs while downloading a track.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// The exception that occurred
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// The current state of the item being downloaded.
        /// </summary>
        public TrackDownloadEventArgs CurrentState { get; set; }
        /// <summary>
        /// What the downloader should advance to when the event handler returns.
        /// </summary>
        public ExceptionSkip SkipTo { get; set; }
    }

    public class MediaDownloadQueue : List<EnqueuedCollection>
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public EnqueuedCollection Enqueue(MusicService service, IMediaCollection collection, string pathFormat)
        {
            var item = new EnqueuedCollection
            {
                Service = service,
                PathFormat = pathFormat,
                MediaCollection = collection
            };
            Add(item);
            return item;
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

        public event EventHandler<ExceptionEventArgs> Exception;

        protected void OnException(ExceptionEventArgs e)
        {
            Exception?.Invoke(this, e);

        }
        #endregion

        private ExceptionSkip skip;

        public async Task StartDownloadAsync()
        {
            var queueView = new Queue<EnqueuedCollection>(this);
            while (queueView.Count > 0)
            {
                var currentItem = queueView.Dequeue();
                OnCollectionDequeued(new CollectionDownloadEventArgs
                {
                    Collection = currentItem,
                    CurrentCollectionIndex = (Count - queueView.Count) - 1,
                    TotalNumberOfCollections = Count
                });
                if (await DownloadCollectionAsync(currentItem)) continue;
                if (skip == ExceptionSkip.Fail)
                {
                    return;
                }
            }
        }

        public EnqueuedCollection ItemById(string id)
        {
            return (from item in this
                    where item.MediaCollection.Id == id
                    select item).FirstOrDefault();

        }

        private void EnsureParentDirectories(string path)
        {
            var parentPath = Path.GetDirectoryName(path);
            if (parentPath == null) return;
            Directory.CreateDirectory(parentPath);
        }

        private async Task<bool> DownloadCollectionAsync(EnqueuedCollection collection)
        {
            var tracksCollectionLength = collection.MediaCollection.Tracks.Count;
            var tracksQueue = new Queue<Track>(collection.MediaCollection.Tracks);
            while (tracksQueue.Count > 0)
            {
                var currentItem = tracksQueue.Dequeue();
                var eventArgs = new TrackDownloadEventArgs
                {
                    CurrentItemIndex = (tracksCollectionLength - tracksQueue.Count) - 1,
                    PercentCompleted = 0M,
                    State = DownloadState.PreProcess,
                    TotalItems = tracksCollectionLength,
                    TrackFile = null
                };
                OnTrackDequeued(eventArgs);

                try
                {
                    if (!currentItem.IsDownloadable)
                    {
                        continue;
                    }
                    OnTrackDownloadProgress(eventArgs);
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
                        eventArgs.State = DownloadState.PostProcess;
                        OnTrackDownloadProgress(eventArgs);
                    };
                    var path = eventArgs.TrackFile.GetPath(collection.PathFormat);
                    EnsureParentDirectories(path);
                    eventArgs.State = DownloadState.Downloading;
                    await downloader.DownloadAsyncTask(eventArgs.TrackFile, path);
                    // Attempt to dispose the downloader, since the most probable case will be that it will
                    // implement IDisposable if it uses sockets
                    var disposableDownloader = downloader as IDisposable;
                    disposableDownloader?.Dispose();
                    // Write the tag
                    eventArgs.State = DownloadState.WritingTags;
                    OnTrackDownloadProgress(eventArgs);
                    TrackTagger.Write(path, currentItem);
                    OnTrackDownloadCompleted(eventArgs);

                }
                catch (Exception ex)
                {
#if DEBUG
                    throw ex;
#else
                    var exEventArgs = new ExceptionEventArgs { CurrentState = eventArgs, Exception = ex };
                    OnException(exEventArgs);
                    switch (exEventArgs.SkipTo)
                    {
                        case ExceptionSkip.Item:
                            continue;

                        case ExceptionSkip.Collection:
                        case ExceptionSkip.Fail:
                            skip = exEventArgs.SkipTo;
                            return false;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
#endif
                }
                
            }
            return true;
        }
    }
}
