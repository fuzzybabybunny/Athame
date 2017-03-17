using System;
using System.Windows.Forms;
using Athame.UI;
using OpenTidl.Enums;

namespace Athame.TidalApi
{
    internal partial class TidalSettingsControl : UserControl
    {
        public TidalSettingsControl()
        {
            InitializeComponent();
        }

        private readonly TidalServiceSettings settings;

        public TidalSettingsControl(TidalServiceSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            var rbem = new RadioButtonEnumMapper();

            rbem.Assign(qLosslessRadioButton, (int)SoundQuality.LOSSLESS);
            rbem.Assign(qHighRadioButton, (int)SoundQuality.HIGH);
            rbem.Assign(qLowRadioButton, (int)SoundQuality.LOW);

            rbem.Select((int)settings.StreamQuality);
            appendVerCheckBox.Checked = settings.AppendVersionToTrackTitle;
            unlessAlbumVersionCheckBox.Enabled = appendVerCheckBox.Checked;
            unlessAlbumVersionCheckBox.Checked = settings.DontAppendAlbumVersion;
            useOfflineUrlEndpointCheckbox.Checked = settings.UseOfflineUrl;

            rbem.ValueChanged += (sender, args) => settings.StreamQuality = (SoundQuality)rbem.Value;

        }

        private void appendVerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            settings.AppendVersionToTrackTitle = appendVerCheckBox.Checked;
            unlessAlbumVersionCheckBox.Enabled = appendVerCheckBox.Checked;
        }

        private void unlessAlbumVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            settings.DontAppendAlbumVersion = unlessAlbumVersionCheckBox.Checked;
        }

        private void useOfflineUrlEndpointCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseOfflineUrl = useOfflineUrlEndpointCheckbox.Checked;
        }
    }
}
