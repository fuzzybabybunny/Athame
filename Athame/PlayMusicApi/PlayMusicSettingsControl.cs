using System.Windows.Forms;
using Athame.UI;
using GoogleMusicApi.Structure.Enums;

namespace Athame.PlayMusicApi
{
    internal partial class PlayMusicSettingsControl : UserControl
    {
        public PlayMusicSettingsControl()
        {
            InitializeComponent();
        }

                private PlayMusicServiceSettings settings;

        public PlayMusicSettingsControl(PlayMusicServiceSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            var rbem = new RadioButtonEnumMapper();

            rbem.Assign(qHighRadioButton, (int)StreamQuality.High );
            rbem.Assign(qMediumRadioButton, (int)StreamQuality.Medium);
            rbem.Assign(qLowRadioButton, (int)StreamQuality.Low);

            rbem.Select((int)settings.StreamQuality);

            rbem.ValueChanged += (sender, args) => settings.StreamQuality = (StreamQuality)rbem.Value;

        }

    }
}
