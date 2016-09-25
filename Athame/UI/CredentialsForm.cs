using System;
using System.Diagnostics;
using System.Windows.Forms;
using Athame.CommonModel;
using Athame.TidalApi;

namespace Athame.UI
{

    public partial class CredentialsForm : Form
    {
        private readonly Service svc;

        private const string TidalMessage =
            "Enter your Tidal account credentials below.";

        private const string AppPasswordUrl =
            "https://security.google.com/settings/security/apppasswords";

        public CredentialsForm(Service service)
        {
            InitializeComponent();
            svc = service;
            if (svc is TidalService)
            {
                helpLabel.Text = TidalMessage;
                apLinkLabel.Visible = false;
                emailLabel.Text = "Username:";
            }
        }

        public AuthenticationResponse Result;

        private void apLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AppPasswordUrl);
        }

        private async void okButton_Click(object sender, EventArgs e)
        {
            using (var waitForm = new WaitForm(String.Format("Signing into {0}...", svc.Name)))
            {
                waitForm.Show(this);
                Result = await svc.LoginAsync(emailTextBox.Text, passwordTextBox.Text);
                waitForm.Close();
                if (Result != null)
                {
                    svc.Settings.Response = Result;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    errorLabel.Text = "An error occurred while signing in. Please check your credentials and try again.";
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
