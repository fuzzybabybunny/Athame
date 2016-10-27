using System;
using System.Drawing;
using System.Windows.Forms;
using Athame.Properties;

namespace Athame.UI
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            saveLocLabel.Text = ApplicationSettings.Default.SaveLocation;
            pathFormatTextBox.Text = ApplicationSettings.Default.TrackFilenameFormat;

            var services = ServiceCollection.Default;
            foreach (var service in services)
            {
                servicesListBox.Items.Add(service.Name);
            }
            if (servicesListBox.Items.Count > 0) servicesListBox.SelectedIndex = 0;
        }

        private void saveLocBrowseButton_Click(object sender, EventArgs e)
        {
            if (mFolderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            ApplicationSettings.Default.SaveLocation = mFolderBrowserDialog.SelectedPath;
            saveLocLabel.Text = ApplicationSettings.Default.SaveLocation;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ApplicationSettings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void pathFormatTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplicationSettings.Default.TrackFilenameFormat = pathFormatTextBox.Text;
        }

        private void servicesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ss = ServiceCollection.Default[servicesListBox.SelectedIndex];
            serviceUiPanel.Controls.Clear();
            serviceUiPanel.Controls.Add(ss.GetSettingsControl());
        }
    }
}
