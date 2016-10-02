using System;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Athame.CommonModel;
using Athame.TidalApi;

namespace Athame.UI
{

    public partial class CredentialsForm : Form
    {
        private readonly Service svc;

        public CredentialsForm(Service service)
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
                    Text = linkPair.Key,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    UseVisualStyleBackColor = true
                };
                // "This will work" - Joe, 28/09/16
                // ReSharper disable once AccessToForEachVariableInClosure
                button.Click += (sender, args) => Process.Start(linkPair.Value);
                linksPanel.Controls.Add(button);
            }
        }

        public AuthenticationResponse Result;

        private async void okButton_Click(object sender, EventArgs e)
        {
            using (var waitForm = new WaitForm(this, String.Format("Signing into {0}...", svc.Name)))
            {
                waitForm.Show(this);
                Result = await svc.LoginAsync(emailTextBox.Text, passwordTextBox.Text);
                waitForm.Close();
                if (Result != null)
                {
                    svc.Settings.Response = Result;
                    ApplicationSettings.Default.Save();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    errorLabel.Text = "An error occurred while signing in. Please check your credentials and try again.";
                    SystemSounds.Hand.Play();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
