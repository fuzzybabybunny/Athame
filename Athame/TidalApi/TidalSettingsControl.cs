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

        private TidalServiceSettings settings;

        public TidalSettingsControl(TidalServiceSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            var rbem = new RadioButtonEnumMapper();

            rbem.Assign(qLosslessRadioButton, (int)SoundQuality.LOSSLESS);
            rbem.Assign(qHighRadioButton, (int)SoundQuality.HIGH);
            rbem.Assign(qLowRadioButton, (int)SoundQuality.LOW);

            rbem.Select((int)settings.StreamQuality);

            rbem.ValueChanged += (sender, args) => settings.StreamQuality = (SoundQuality)rbem.Value;

        }
    }
}
