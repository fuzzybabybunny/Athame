using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace Athame.DownloadAndTag
{
    public class MediaDownloadQueue
    {
        private List<EnqueuedTrack> mList = new List<EnqueuedTrack>();

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

        public async Task StartDownloadAsync()
        {
            
        }
    }
}
