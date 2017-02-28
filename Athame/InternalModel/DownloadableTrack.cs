using System;
using Athame.PluginAPI.Service;
using Athame.Utils;
using SysIOPath = System.IO.Path;

namespace Athame.InternalModel
{
    [Obsolete]
    public enum TrackState
    {
        Ready,
        DownloadingArtwork,
        DownloadingTrack,
        Tagging,
        Complete
    }

    /// <summary>
    /// Represents a track that can be downloaded.
    /// </summary>
    [Obsolete]
    public class DownloadableTrack
    {
        public const string ArtworkName = "cover.jpg";

        public Track CommonTrack { get; set; }

        /// <summary>
        /// The track's current download state.
        /// </summary>
        public TrackState State { get; set; }

        /// <summary>
        /// If the track should be downloaded.
        /// </summary>
        public bool WillDownload { get; set; }

        /// <summary>
        /// The track's absolute path. To set using a format string, use <see cref="SetPathFromFormat"/>.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The track's artwork path, created by appending the parent directory of <see cref="Path"/> with <see cref="ArtworkName"/>,
        /// or null if <see cref="Path"/> is null.
        /// </summary>
        public string ArtworkPath {
            get
            {
                return Path == null ? null : SysIOPath.Combine(SysIOPath.GetDirectoryName(Path) ?? "", ArtworkName);
            } 
        }

        public static DownloadableTrack FromCommon(string pathFormat, Track t)
        {
            var dlt = new DownloadableTrack
            {
                CommonTrack = t,
                State = TrackState.Ready,
                WillDownload = t.IsDownloadable
            };
            dlt.SetPathFromFormat(pathFormat);
            return dlt;
        }

        /// <summary>
        /// Sets the track's path using a format string.
        /// </summary>
        /// <param name="pathFormat">The format string to use. Must be absolute.</param>
        public void SetPathFromFormat(string pathFormat)
        {
            Path = FormatTrackPath(pathFormat, CommonTrack);
        }

        public static string FormatTrackPath(string pathFormat, Track track)
        {
            return null;
        }
    }
}
