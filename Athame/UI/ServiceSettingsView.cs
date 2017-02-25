using System;
using System.Windows.Forms;
using Athame.PluginAPI.Service;

namespace Athame.UI
{
    public partial class ServiceSettingsView : UserControl
    {

        private readonly SplitStringParser sspSignInStatus, sspSignInButton;
        private readonly MusicService service;

        public ServiceSettingsView(MusicService service)
        {
            this.service = service;
            InitializeComponent();
            sspSignInStatus = new SplitStringParser(signInStatusLabel);
            sspSignInButton = new SplitStringParser(signInButton);
            if (service.IsAuthenticated)
            {
                signInStatusLabel.Text = String.Format(sspSignInStatus.Get(service.IsAuthenticated),
                    service.Settings.Response.UserName);
            }
            else
            {
                sspSignInStatus.Update(false);
            }
            sspSignInButton.Update(service.IsAuthenticated);
            mLayout.Controls.Add(service.GetSettingsControl(), 0, 1);
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            if (!service.IsAuthenticated)
            {
                var dlg = new CredentialsForm(service);
                var result = dlg.ShowDialog();
                if (result != DialogResult.OK) return;
                signInStatusLabel.Text = String.Format(sspSignInStatus.Get(true), dlg.Result.UserName);
                sspSignInButton.Update(true);
            }
            else
            {
                service.ClearSession();
                sspSignInStatus.Update(false);
                sspSignInButton.Update(false);
            }

        }
    }
}
