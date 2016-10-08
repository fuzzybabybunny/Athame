using System;
using System.Drawing;
using System.Windows.Forms;
using Athame.CommonModel;
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
            // this is why I should've used WPF in the first place lol
            // http://stackoverflow.com/questions/1532301/visual-studio-tabcontrol-tabpages-insert-not-working
            // ReSharper disable once UnusedVariable
            var _ = tabControl1.Handle;
            foreach (var service in services)
            {
                var tab = new TabPage(service.Name);
                tab.Controls.Add(new ServiceSettingsView(service));
                // insert after general tab - this is here only so tabs can be offset
                // in the case of a new static tab being added in designer
                tabControl1.TabPages.Insert(tabControl1.TabCount, tab);
            }
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
    }
}
