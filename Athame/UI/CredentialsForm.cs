using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using Athame.PluginAPI.Service;

namespace Athame.UI
{

    public partial class CredentialsForm : Form
    {
        private readonly MusicService svc;

        public CredentialsForm(MusicService service)
        {
            InitializeComponent();
            svc = service;
            FillInInfo();
        }

        private void FillInInfo()
        {
            Text = String.Format(Text, svc.Name);
            if (svc.Flow == null) return;
            helpLabel.Text = svc.Flow.SignInInformation ?? String.Format(helpLabel.Text, svc.Name);
            foreach (var linkPair in svc.Flow.LinksToDisplay)
            {
                var button = new Button
                {
                    Text = linkPair.DisplayName,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    UseVisualStyleBackColor = true
                };
                urlToolTip.SetToolTip(button, linkPair.Link.ToString());
                // "This will work" - Joe, 28/09/16
                // ReSharper disable once AccessToForEachVariableInClosure
                button.Click += (sender, args) => Process.Start(linkPair.Link.ToString());
                linksPanel.Controls.Add(button);
            }
        }

        public AuthenticationResponse Result;

        private void okButton_Click(object sender, EventArgs e)
        {
            var waitForm = CommonTaskDialogs.Wait(this, $"Signing into {svc.Name}...");
            waitForm.Opened += async (o, args) => {
                Result = await svc.LoginAsync(emailTextBox.Text, passwordTextBox.Text);
                waitForm.Close();
                if (Result != null)
                {
                    svc.Settings.Response = Result;
                    Program.DefaultSettings.Save();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    errorLabel.Text = "An error occurred while signing in. Please check your credentials and try again.";
                    SystemSounds.Hand.Play();
                }
            };
            waitForm.Show();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
