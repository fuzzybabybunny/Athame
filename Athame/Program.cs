using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Athame.PluginAPI;
using Athame.Settings;
using Athame.UI;

namespace Athame
{
    public static class Program
    {
        private const string SettingsFilename = "settings.json";
        private static string SettingsPath;

        public static AthameApplication DefaultApp;
        public static SettingsManager<AthameSettings> DefaultSettings;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Create app instance config
            DefaultApp = new AthameApplication
            {
                IsWindowed = true,
#if DEBUG
                UserDataPath = Path.Combine(Directory.GetCurrentDirectory(), "UserDataDebug")
#else
                UserDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Athame")
#endif
            };

            // Ensure user data dir
            Directory.CreateDirectory(DefaultApp.UserDataPath);

            // Load settings
            SettingsPath = DefaultApp.UserDataPathOf(SettingsFilename);
            DefaultSettings = new SettingsManager<AthameSettings>(SettingsPath);
            DefaultSettings.Load();

            // Begin main form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
