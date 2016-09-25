using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Athame.CommonModel;
using Athame.DownloadAndTag;
using Album = Athame.CommonModel.Album;
using Track = Athame.CommonModel.Track;

namespace Athame.UI
{
    public partial class MainForm : Form
    {
        private readonly Logger logger;
        
        public MainForm()
        {
            InitializeComponent();
            logger = new FormsLogger(logTextBox, totalProgressStatus);
            logger.Info("Ready");
        }

        private void SetAlbum(Album album)
        {
            logger.Info("Album loaded: " + album.Title);
            queueListView.Items.Clear();
            foreach (var track in album.Tracks)
            {
                var item = new ListViewItem();
                item.SubItems.Add(track.TrackNumber.ToString());
                item.SubItems.Add(track.Title);
                item.SubItems.Add(track.Artist);
                item.SubItems.Add(track.Album.Title);
                queueListView.Items.Add(item);
            }
        }

        private void LockUi()
        {
            queueListView.Enabled = false;
            idTextBox.Enabled = false;
            settingsButton.Enabled = false;
        }

        private void UnlockUi()
        {
            queueListView.Enabled = true;
            idTextBox.Enabled = true;
            settingsButton.Enabled = true;
        }

        private void PrepareForNextTrack(Track track, int current, int count)
        {
            // Put leading zero in front of track number
            var fmt = String.Format("[{0}/{1}] {2:D2}: {3} - {4} - {5}",
                                  current + 1, count, track.TrackNumber, track.Title, track.Artist, track.Album.Title);
            currTrackLabel.Text = fmt;
            logger.Info(fmt);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var url = new Uri(idTextBox.Text);
            var service = ServiceCollection.Default.GetByHost(url.Host);

            // #1: Is hostname valid?
            if (service == null)
            {
                logger.Error(url.Host + ": No service found for host.");
                MessageBox.Show("GPMDL cannot download this URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            logger.Info("Using service " + service.Name);

            // #2: Is user authenticated?
            if (!service.IsAuthenticated)
            {
                logger.Info("Not authenticated!");
                MessageBox.Show(
                    String.Format(
                        "You must sign into the {0} service. Click the Settings button, click the tab named \"{0}\", then click the Sign In button.", service.Name), 
                        "Not signed in",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return;
            }

            // Parse URL
            UrlParseResult result;
            try
            {
                result = service.ParseUrl(url);
            }
            catch (InvalidServiceUrlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            logger.Debug("Id = " + result.Id + ", Method = " + result.Type);
            currTrackLabel.Text = "Warming up...";
            var tagger = new TrackTagger();

            // #3: Is the type correct and supported?
            switch (result.Type)
            {
                case UrlContentType.Album:
                    try
                    {
                        LockUi();
                        logger.Info("Getting album info...");
                        // Get album and display it in listview
                        var album = await service.GetAlbumWithTracksAsync(result.Id);
                        SetAlbum(album);
                        // Init downloader
                        var absPathFormat = Path.Combine(
                            ApplicationSettings.Default.SaveLocation,
                            ApplicationSettings.Default.TrackFilenameFormat);
                        var downloader = new TrackDownloader(service, album.Tracks, absPathFormat);
                        totalProgressBar.Value = 0;
                        PrepareForNextTrack(album.Tracks[0], 0, album.Tracks.Count);
                        downloader.ItemProgressChanged += (o, args) =>
                        {
                            switch (args.Stage)
                            {
                                case ProgressStage.Artwork:
                                    totalProgressStatus.Text = "Getting artwork...";
                                    break;
                                case ProgressStage.DownloadUrl:
                                    totalProgressStatus.Text = "Getting download URL...";
                                    break;
                                case ProgressStage.Track:
                                    totalProgressStatus.Text = "Getting track...";
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            totalProgressBar.Value = args.OverallCompletionPercentage;
                        };
                        downloader.ItemDownloadCompleted += (o, args) =>
                        {
                            logger.Info("Download complete");
                            totalProgressStatus.Text = "Tagging...";
                            tagger.Write(args.DestinationPath, album.Tracks[args.CurrentItem], args.CoverArtPath);
                            logger.Info("Tagged track");
                            var nextIndex = args.CurrentItem + 1;
                            if (nextIndex < args.TotalItems)
                            {
                                PrepareForNextTrack(album.Tracks[nextIndex], nextIndex, args.TotalItems);
                            }
                        };
                        await downloader.DownloadAsync();
                        currTrackLabel.Text = GetCompletionMessage();
                        totalProgressStatus.Text = "Downloaded album successfully";
                        logger.Info("Downloaded album successfully");
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        
                        MessageBox.Show(
                            ex.GetType().Name + ": " + ex.Message + "\nConsult the error log for more information",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        UnlockUi();
                    }
                    break;

                default:
                    MessageBox.Show(String.Format("{0}s are not supported yet.", result.Type), "URL Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }
        }

        #region Easter egg

        private readonly string[] messages = {"Woo-hoo!", "We did it!", "Yusssss", "Alright!", "Sweet!", "Nice...."};
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

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            foreach (var service in ServiceCollection.Default)
            {
                if (service.Settings.Response == null) return;
                var result = false;
                using (var waitForm = new WaitForm(String.Format("Signing into {0}...", service.Name)))
                {
                    waitForm.Show(this);
                    try
                    {

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
                            logger.Info(service.Name + ": Signed in successfully");
                        }
                        else
                        {
                            logger.Error(service.Name + ": Failed to sign in");
                        }
                    }
                }
            }
            Visible = true;
        }

        private class FormsLogger : Logger
        {
            private readonly TextBox textBox;
            private readonly Label infoLabel;

            public FormsLogger(TextBox textBox, Label infoLabel)
            {
                this.textBox = textBox;
                this.infoLabel = infoLabel;
            }

            public override void Write(MessageLevel level, string line)
            {
                StringBuilder sb = new StringBuilder();
                switch (level)
                {
                    case MessageLevel.Info:
                        sb.Append("[Info] ");
                        break;
                    case MessageLevel.Debug:
                        sb.Append("[Debug] ");
                        break;
                    case MessageLevel.Warning:
                        sb.Append("[Warning] ");
                        break;
                    case MessageLevel.Error:
                        sb.Append("[Error] ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("level", level, null);
                }
                sb.Append(line);
                sb.Append(Environment.NewLine);
                if (textBox.InvokeRequired)
                {
                    textBox.BeginInvoke(new Action(() =>
                    {
                        textBox.Text += (sb);
                        textBox.SelectionStart = textBox.TextLength;
                        textBox.ScrollToCaret();
                    }));
                }
                else
                {
                    textBox.Text += (sb);
                    textBox.SelectionStart = textBox.TextLength;
                    textBox.ScrollToCaret();
                }
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationSettings.Default.Save();
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            dlButton.Enabled = !String.IsNullOrEmpty(idTextBox.Text);
        }
    }
}
