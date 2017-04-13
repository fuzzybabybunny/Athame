using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.DownloadAndTag;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using Athame.PluginManager;
using Athame.Properties;
using Athame.Settings;
using Athame.UI.Win32;
using Athame.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Athame.UI
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Internal class for keeping track of an individual listitem's relation
        /// </summary>
        private class MediaItemTag
        {
            public EnqueuedCollection Collection { get; set; }
            public Track Track { get; set; }
            public int IndexInCollection { get; set; }
            public int GroupIndex { get; set; }
            public int GlobalItemIndex { get; set; }
        }

        // Constants
        private const string GroupHeaderFormat = "{0}: {1} ({2})";

        // Read-only instance vars
        private readonly TaskbarManager mTaskbarManager = TaskbarManager.Instance;
        private readonly MediaDownloadQueue mediaDownloadQueue = new MediaDownloadQueue();

        // Instance vars
        private UrlParseResult mResult;
        private MusicService mService;
        private ListViewItem mCurrentlySelectedQueueItem;
        private ListViewItem currentlyDownloadingItem;
        private CollectionDownloadEventArgs currentCollection;
        

        public MainForm()
        {
            InitializeComponent();
            UnlockUi();
            // The formula (1 / x) * 1000 where x = FPS will give us our timer interval in regards
            // to how fast we want the animation to show in FPS
            queueImageAnimationTimer.Interval = (int)(((double)1 / 12) * 1000);

            // Add event handlers for MDQ
            mediaDownloadQueue.Exception += MediaDownloadQueue_Exception;
            mediaDownloadQueue.CollectionDequeued += MediaDownloadQueue_CollectionDequeued;
            mediaDownloadQueue.TrackDequeued += MediaDownloadQueue_TrackDequeued;
            mediaDownloadQueue.TrackDownloadCompleted += MediaDownloadQueue_TrackDownloadCompleted;
            mediaDownloadQueue.TrackDownloadProgress += MediaDownloadQueue_TrackDownloadProgress;
        }

        /// <summary>
        /// Rounds a decimal less than 1 up then multiplies it by 100. If the result is greater than 100, returns 100, otherwise returns the result.
        /// </summary>
        /// <param name="percent">The percent value to convert.</param>
        /// <returns>An integer that is not greater than 100.</returns>
        private int PercentToInt(decimal percent)
        {
            var rounded = (int) (Decimal.Round(percent, 2, MidpointRounding.ToEven) * (decimal) 100);
            return rounded > 100 ? 100 : rounded;
        }

        private void MediaDownloadQueue_TrackDownloadProgress(object sender, TrackDownloadEventArgs e)
        {
            collectionProgressBar.Value = PercentToInt(e.TotalProgress);
            totalProgressBar.Value +=
                PercentToInt(((decimal) (e.TotalProgress + currentCollection.CurrentCollectionIndex) /
                              currentCollection.TotalNumberOfCollections)) - totalProgressBar.Value;
            SetGlobalProgress(totalProgressBar.Value);
            switch (e.State)
            {
                case DownloadState.PreProcess:
                    StartAnimation(currentlyDownloadingItem);
                    currentlyDownloadingItem.Text = "Downloading...";
                    collectionStatusLabel.Text = "Pre-processing...";
                    break;
                case DownloadState.DownloadingAlbumArtwork:
                    collectionStatusLabel.Text = "Downloading album artwork...";
                    break;
                case DownloadState.Downloading:
                    collectionStatusLabel.Text = "Downloading track...";
                    break;
                case DownloadState.PostProcess:
                    collectionStatusLabel.Text = "Post-processing...";
                    break;
                case DownloadState.WritingTags:
                    collectionStatusLabel.Text = "Writing tags...";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MediaDownloadQueue_TrackDownloadCompleted(object sender, TrackDownloadEventArgs e)
        {
            collectionStatusLabel.Text = "Completed";
            StopAnimation();
            currentlyDownloadingItem.ImageKey = "done";
            currentlyDownloadingItem.Text = "Completed";
        }

        private void MediaDownloadQueue_TrackDequeued(object sender, TrackDownloadEventArgs e)
        {
            // this'll bite me in the ass someday
            currentlyDownloadingItem = queueListView.Groups[currentCollection.CurrentCollectionIndex].Items[e.CurrentItemIndex * 2];
        }

        private void MediaDownloadQueue_CollectionDequeued(object sender, CollectionDownloadEventArgs e)
        {
            currentCollection = e;
            totalStatusLabel.Text = $"{e.CurrentCollectionIndex + 1}/{e.TotalNumberOfCollections}: {MediaCollectionAsType(e.Collection.MediaCollection)} \"{e.Collection.MediaCollection.Title}\"";
        }

        private void MediaDownloadQueue_Exception(object sender, ExceptionEventArgs e)
        {
            throw e.Exception;
        }

        #region Download queue manipulation
        private void AddToQueue(MusicService service, IMediaCollection item, string pathFormat)
        {
            var enqueuedItem = mediaDownloadQueue.Enqueue(service, item, pathFormat);
            var mediaType = MediaCollectionAsType(item);
            var header = String.Format(GroupHeaderFormat, mediaType, item.Title, service.Name);
            var group = new ListViewGroup(header);
            var groupIndex = queueListView.Groups.Add(group);
            for (var i = 0; i < item.Tracks.Count; i++)
            {
                var t = item.Tracks[i];
                var lvItem = new ListViewItem
                {
                    Group = group,
                    Tag = new MediaItemTag
                    {
                        Track = t,
                        Collection = enqueuedItem,
                        GroupIndex = groupIndex,
                        IndexInCollection = i
                    }
                };
                if (!t.IsDownloadable)
                {
                    lvItem.BackColor = SystemColors.Control;
                    lvItem.ForeColor = SystemColors.GrayText;
                    lvItem.ImageKey = "not_downloadable";
                    lvItem.Text = "Unavailable";
                }
                else
                {
                    lvItem.ImageKey = "ready";
                    lvItem.Text = "Ready to download";
                }
                lvItem.SubItems.Add(t.DiscNumber + " / " + t.TrackNumber);
                lvItem.SubItems.Add(t.Title);
                lvItem.SubItems.Add(t.Artist);
                lvItem.SubItems.Add(t.Album.Title);
                lvItem.SubItems.Add(t.GetBasicPath(enqueuedItem.PathFormat));
                group.Items.Add(lvItem);
                queueListView.Items.Add(lvItem);
            }
        }

        private void RemoveCurrentlySelectedTracks()
        {
            if (mCurrentlySelectedQueueItem == null) return;
            var selectedItemsList = queueListView.SelectedItems.Cast<ListViewItem>().ToList();
            foreach (var listViewItem in selectedItemsList)
            {
                var item = (MediaItemTag)listViewItem.Tag;
                item.Collection.MediaCollection.Tracks.Remove(item.Track);
                queueListView.Items.Remove(listViewItem);
                if (item.Collection.MediaCollection.Tracks.Count == 0)
                {
                    mediaDownloadQueue.Remove(item.Collection);
                }
            }
            mCurrentlySelectedQueueItem = null;
        }

        private void RemoveCurrentlySelectedGroup()
        {
            if (mCurrentlySelectedQueueItem == null) return;
            // Remove internal queue item
            var item = (MediaItemTag)mCurrentlySelectedQueueItem.Tag;
            mediaDownloadQueue.Remove(item.Collection);
            // We have to remove the group from the ListView first...
            var group = mCurrentlySelectedQueueItem.Group;
            queueListView.Groups.Remove(group);
            // ...then remove all the items
            foreach (var groupItem in group.Items)
            {
                queueListView.Items.Remove((ListViewItem)groupItem);
            }
            mCurrentlySelectedQueueItem = null;
        }
        #endregion

        private MediaType MediaCollectionAsType(IMediaCollection collection)
        {
            if (collection is Album)
            {
                return MediaType.Album;
            }
            if (collection is Playlist)
            {
                return MediaType.Playlist;
            }
            if (collection is SingleTrackCollection)
            {
                return MediaType.Track;
            }
            return MediaType.Unknown;
        }

        private void LockUi()
        {
            idTextBox.Enabled = false;
            dlButton.Enabled = false;
            settingsButton.Enabled = false;
            pasteButton.Enabled = false;
            clearButton.Enabled = false;
            startDownloadButton.Enabled = false;
            queueListView.Enabled = false;
        }

        private void UnlockUi()
        {
            idTextBox.Enabled = true;
            dlButton.Enabled = true;
            settingsButton.Enabled = true;
            pasteButton.Enabled = true;
            clearButton.Enabled = true;
            startDownloadButton.Enabled = mediaDownloadQueue.Count > 0;
            queueListView.Enabled = true;
        }

        private void SetGlobalProgress(int value)
        {
            if (value == 0)
            {
                mTaskbarManager.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
            mTaskbarManager?.SetProgressValue(value, totalProgressBar.Maximum);
        }

        private void SetGlobalProgressState(ProgressBarState state)
        {
            switch (state)
            {
                case ProgressBarState.Normal:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Normal);
                    break;
                case ProgressBarState.Error:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Error);
                    break;
                case ProgressBarState.Warning:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Paused);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void PresentException(Exception ex)
        {
#if DEBUG
            throw ex;
#else
            SetGlobalProgressState(ProgressBarState.Error);
            var th = "An unknown error occurred";
            if (ex is ResourceNotFoundException)
            {
                th = "Resource not found";
            }
            else if (ex is InvalidSessionException)
            {
                th = "Invalid session/subscription expired";
            }
            CommonTaskDialogs.Error(ex, th).Show();
#endif
        }

        private void OpenInExplorer(string directory)
        {
            Process.Start($"\"{directory}\"");
        }

        private async Task DownloadQueue()
        {
            try
            {

                LockUi();
                totalStatusLabel.Text = "Warming up...";
                await mediaDownloadQueue.StartDownloadAsync();
                currentlyDownloadingItem = null;
                SetGlobalProgress(0);
                SetGlobalProgressState(ProgressBarState.Normal);

            }
            catch (Exception ex)
            {
                PresentException(ex);

            }
            finally
            {
                UnlockUi();
            }
        }

        private MediaTypeSavePreference PreferenceForType(MediaType type)
        {
            if (type == MediaType.Playlist && !Program.DefaultSettings.Settings.PlaylistSavePreferenceUsesGeneral)
            {
                return Program.DefaultSettings.Settings.PlaylistSavePreference;
            }
            return Program.DefaultSettings.Settings.GeneralSavePreference.Clone();
        }

        //        private async Task DownloadTracks(MusicService svc, List<DownloadableTrack> tracks)
        //        {
        //            var tagger = new TrackTagger();
        //            var downloader = new TrackDownloader(svc, tracks, mPathFormat);
        //            totalProgressBar.Value = 0;
        //            PrepareForNextTrack(tracks[0].CommonTrack, 0, tracks.Count);
        //            downloader.ItemProgressChanged += (o, args) =>
        //            {
        //                switch (args.CurrentTrack.State)
        //                {
        //                    case TrackState.DownloadingArtwork:
        //                        totalProgressStatus.Text = "Getting artwork...";
        //                        break;
        //                    case TrackState.DownloadingTrack:
        //                        totalProgressStatus.Text = "Getting track...";
        //                        break;
        //                    default:
        //                        throw new ArgumentOutOfRangeException();
        //                }
        //                SetGlobalProgress(args.OverallCompletionPercentage);
        //            };
        //            downloader.ItemDownloadCompleted += (o, args) =>
        //            {
        //                totalProgressStatus.Text = "Tagging...";
        //                tagger.Write(args.CurrentTrack.Path, tracks[args.CurrentItemIndex].CommonTrack, args.CurrentTrack.ArtworkPath);
        //                var lvItemIndex = mGroupAndQueueIndices[currentCollection][args.CurrentItemIndex];
        //                var lvItem = queueListView.Items[lvItemIndex];
        //                lvItem.Checked = false;
        //                lvItem.ImageKey = "done";
        //                var nextIndex = args.CurrentItemIndex + 1;
        //                if (nextIndex < args.TotalItems)
        //                {
        //                    PrepareForNextTrack(tracks[nextIndex].CommonTrack, nextIndex, args.TotalItems);
        //                }
        //            };
        //            await downloader.DownloadAsync();
        //            currTrackLabel.Text = GetCompletionMessage();
        //            totalProgressStatus.Text = "Downloaded album successfully";
        //        }

        private static bool IsWithinVisibleBounds(Point topLeft)
        {
            var screens = Screen.AllScreens;
            return (from screen in screens
                    where screen.WorkingArea.Contains(topLeft)
                    select screen).Any();
        }

        private void ShowStartupTaskDialog()
        {
            var td = CommonTaskDialogs.Wait(this, null);
            var openCt = new CancellationTokenSource();
            td.Opened += async (o, args) =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    foreach (var service in ServiceRegistry.Default)
                    {
                        if (service.Settings.Response == null) continue;
                        var result = false;
                        td.InstructionText = $"Signing into {service.Name}...";
                        td.Text = $"Signing in as {service.Settings.Response.UserName}";
                        try
                        {
                            openCt.Token.ThrowIfCancellationRequested();
                            result = await service.RestoreSessionAsync(service.Settings.Response);
                        }
                        catch (NotImplementedException)
                        {
                            result = service.RestoreSession(service.Settings.Response);
                        }
                        finally
                        {
                            if (result)
                            {
                            }
                            else
                            {
                                MessageBox.Show($"Failed to sign in to {service.Name}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    td.Close();
                }, openCt.Token);

            };
            if (td.Show() == TaskDialogResult.Cancel)
            {
                openCt.Cancel(true);
            }
        }

#region Validation for URL
        private const string UrlInvalid = "Invalid URL. Check that the URL begins with \"http://\" or \"https://\".";
        private const string UrlNoService = "Can't download this URL.";
        private const string UrlNeedsAuthentication = "You need to sign in to {0} first. " + UrlNeedsAuthenticationLink1;
        private const string UrlNeedsAuthenticationLink1 = "Click here to sign in.";
        private const string UrlNotParseable = "The URL does not point to a valid track, album, artist or playlist.";
        private const string UrlValidParseResult = "{0} from {1}";

        private bool ValidateEnteredUrl()
        {
            urlValidStateLabel.ResetText();
            urlValidStateLabel.Links.Clear();
            urlValidStateLabel.Image = Resources.error;
            dlButton.Enabled = false;

            // Hide on empty
            if (String.IsNullOrWhiteSpace(idTextBox.Text))
            {
                urlValidStateLabel.Visible = false;
                return false;
            }
            urlValidStateLabel.Visible = true;

            Uri url;
            // Invalid URL
            if (!Uri.TryCreate(idTextBox.Text, UriKind.Absolute, out url))
            {
                urlValidStateLabel.Text = UrlInvalid;
                return false;
            }
            var service = ServiceRegistry.Default.GetByBaseUri(url);
            // No service associated with host
            if (service == null)
            {
                urlValidStateLabel.Text = UrlNoService;
                return false;
            }
            // Not authenticated
            if (!service.IsAuthenticated)
            {
                urlValidStateLabel.Text = String.Format(UrlNeedsAuthentication, service.Name);
                var linkIndex = urlValidStateLabel.Text.LastIndexOf(UrlNeedsAuthenticationLink1, StringComparison.Ordinal);
                urlValidStateLabel.Links.Add(linkIndex, urlValidStateLabel.Text.Length, service);
                return false;
            }
            // URL doesn't point to media
            var result = service.ParseUrl(url);
            if (result == null)
            {
                urlValidStateLabel.Text = UrlNotParseable;
                return false;
            }
            // Success
            urlValidStateLabel.Image = Resources.done;
            urlValidStateLabel.Text = String.Format(UrlValidParseResult, result.Type, service.Name);
            dlButton.Enabled = true;
            mResult = result;
            mService = service;
            return true;
        }
#endregion

#region Easter egg

        private readonly string[] messages = { "Woo-hoo!", "We did it!", "Yusssss", "Alright!", "Sweet!", "Nice...." };
        private readonly Random random = new Random();

        private string GetCompletionMessage()
        {
            var messagesList = messages.ToList();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                messagesList.Add("It's Friday, baby!");
            }
            return messagesList[random.Next(messagesList.Count)];
        }

#endregion

#region MainForm event handlers and control event handlers
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Don't add if the item is already enqueued.
                var isAlreadyInQueue = mediaDownloadQueue.ItemById(mResult.Id) != null;
                if (isAlreadyInQueue)
                {
                    using (
                        var td = CommonTaskDialogs.Message(this, TaskDialogStandardIcon.Error, "Athame",
                            "Cannot add to download queue", "This item already exists in the download queue.",
                            TaskDialogStandardButtons.Ok))
                    {
                        td.Show();
                        return;
                    }
                }

                // Ask for the location if required before we begin retrieval
                var prefType = PreferenceForType(mResult.Type);
                var saveDir = prefType.SaveDirectory;
                if (prefType.AskForLocation)
                {
                    using (var folderSelectionDialog = new FolderBrowserDialog { Description = "Select a destionation for this media:" })
                    {
                        if (folderSelectionDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            saveDir = folderSelectionDialog.SelectedPath;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                // Filter out types we can't process right now
                if (mResult.Type != MediaType.Album && mResult.Type != MediaType.Playlist &&
                    mResult.Type != MediaType.Track)
                {
                    using (var noTypeTd = CommonTaskDialogs.Message(this, TaskDialogStandardIcon.Warning,
                        "Athame", $"'{mResult.Type}' is not supported yet.",
                        "You may be able to download it in a later release.", TaskDialogStandardButtons.Ok))
                    {
                        noTypeTd.Show();
                        return;
                    }
                }

                // Build wait dialog
                var retrievalWaitTaskDialog = new TaskDialog
                {
                    Cancelable = false,
                    Caption = "Athame",
                    InstructionText = $"Getting {mResult.Type.ToString().ToLower()} details...",
                    Text = $"{mService.Name}: {mResult.Id}",
                    StandardButtons = TaskDialogStandardButtons.Cancel,
                    OwnerWindowHandle = Handle,
                    ProgressBar = new TaskDialogProgressBar { State = TaskDialogProgressBarState.Marquee }
                };
                // Open handler
                retrievalWaitTaskDialog.Opened += async (o, args) =>
                {
                    LockUi();
                    var pathFormat = Path.Combine(saveDir, prefType.GetPlatformSaveFormat());
                    switch (mResult.Type)
                    {
                        case MediaType.Album:
                            // Get album and display it in listview
                            var album = await mService.GetAlbumAsync(mResult.Id, true);
                            AddToQueue(mService, album, pathFormat);
                            break;

                        case MediaType.Playlist:
                            // Get playlist and display it in listview
                            var playlist = await mService.GetPlaylistAsync(mResult.Id);
                            AddToQueue(mService, playlist, pathFormat);
                            break;

                        case MediaType.Track:
                            var track = await mService.GetTrackAsync(mResult.Id);
                            AddToQueue(mService, track.AsCollection(), pathFormat);
                            break;
                    }
                    idTextBox.Clear();
                    UnlockUi();
                    retrievalWaitTaskDialog.Close();
                };
                // Show dialog
                retrievalWaitTaskDialog.Show();
            }
            catch (Exception ex)
            {
                PresentException(ex);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!IsWithinVisibleBounds(Program.DefaultSettings.Settings.MainWindowPreference.Location) ||
                Program.DefaultSettings.Settings.MainWindowPreference.Location == new Point(0, 0))
            {
                CenterToScreen();
            }
            else
            {
                Location = Program.DefaultSettings.Settings.MainWindowPreference.Location;
            }

            var savedSize = Program.DefaultSettings.Settings.MainWindowPreference.Size;
            if (savedSize.Width < MinimumSize.Width && savedSize.Height < MinimumSize.Height)
            {
                Program.DefaultSettings.Settings.MainWindowPreference.Size = savedSize = MinimumSize;
            }
            Size = savedSize;
        }



        private void settingsButton_Click(object sender, EventArgs e)
        {
            var absLoc = settingsButton.PointToScreen(new Point(0, settingsButton.Height));
            mMenu.Show(absLoc);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.DefaultSettings.Save();
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateEnteredUrl();
        }

        private void clearButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            idTextBox.Clear();
        }

        private void pasteButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            idTextBox.Clear();
            idTextBox.Paste();
        }

        private void urlValidStateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var svc = (MusicService)e.Link.LinkData;
            using (var cf = new CredentialsForm(svc))
            {
                var res = cf.ShowDialog(this);
                if (res != DialogResult.OK) return;
                ValidateEnteredUrl();
            }

        }

        private async void startDownloadButton_Click(object sender, EventArgs e)
        {
            if (mediaDownloadQueue.Count == 0)
            {
                using (
                    var td = CommonTaskDialogs.Message(this, TaskDialogStandardIcon.Error, "Athame",
                        "No tracks are in the queue.",
                        "You can add tracks by copying the URL to an album, artist, track, or playlist and pasting it into Athame.",
                        TaskDialogStandardButtons.Ok))
                {
                    td.Show();
                }
                return;
            }

            try
            {

                LockUi();
                totalStatusLabel.Text = "Warming up...";
                await mediaDownloadQueue.StartDownloadAsync();
                currentlyDownloadingItem = null;
                SetGlobalProgress(0);
                SystemSounds.Beep.Play();
                this.Flash(FlashMethod.All | FlashMethod.TimerNoForeground, Int32.MaxValue, 0);
            }
            catch (Exception ex)
            {
                PresentException(ex);

            }
            finally
            {
                UnlockUi();
            }

        }

        private void queueListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!queueListView.FocusedItem.Bounds.Contains(e.Location)) return;
            mCurrentlySelectedQueueItem = queueListView.FocusedItem;
            // Only show context menu on right click
            if (e.Button != MouseButtons.Right) return;
            showCollectionInFileBrowserToolStripMenuItem.Enabled = GetCurrentlySelectedItemDir() != null;
            removeTrackToolStripMenuItem.Text = queueListView.SelectedItems.Count == 1 ? "Remove item" : "Remove items";
            queueMenu.Show(Cursor.Position);
        }



        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCurrentlySelectedGroup();
        }

        private void queueListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (queueListView.SelectedIndices.Count == 0) return;
            mCurrentlySelectedQueueItem = queueListView.SelectedItems[0];
        }

        private void queueListView_MouseHover(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog(this);
        }



        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCurrentlySelectedTracks();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            Program.DefaultSettings.Settings.MainWindowPreference.Location = Location;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Program.DefaultSettings.Settings.MainWindowPreference.Size = Size;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LockUi();
            ShowStartupTaskDialog();
            UnlockUi();
        }

        private void queueListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (e.Shift)
                {
                    RemoveCurrentlySelectedGroup();
                }
                else
                {
                    RemoveCurrentlySelectedTracks();
                }

            }
        }
#endregion

        private const int ImageListAnimStartIndex = 4;
        private const int ImageListAnimEndIndex = 15;
        private int currentFrame = ImageListAnimStartIndex;
        private ListViewItem currentAnimatingItem;

        private void queueImageAnimationTimer_Tick(object sender, EventArgs e)
        {
            currentFrame = ++currentFrame > ImageListAnimEndIndex ? ImageListAnimStartIndex : currentFrame;
            currentAnimatingItem.ImageIndex = currentFrame;
        }

        private void StartAnimation(ListViewItem item)
        {
            currentAnimatingItem = item;
            queueImageAnimationTimer.Start();
        }

        private void StopAnimation()
        {
            currentAnimatingItem = null;
            queueImageAnimationTimer.Stop();
        }

        private string GetCurrentlySelectedItemDir()
        {
            if (mCurrentlySelectedQueueItem == null) return null;
            var tag = (MediaItemTag)mCurrentlySelectedQueueItem.Tag;
            var parentDir = Path.GetDirectoryName(tag.Track.GetBasicPath(tag.Collection.PathFormat));
            return Directory.Exists(parentDir) ? parentDir : null;
        }

        private void showCollectionInFileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dir = GetCurrentlySelectedItemDir();
            if (dir == null) return;
            Process.Start($"\"{dir}\"");
        }
    }
}
