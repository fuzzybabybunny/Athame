using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Athame.PluginAPI;
using Athame.UI;

namespace Athame
{
    public static class Program
    {
        public static AthameApplication DefaultApp;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            DefaultApp = new AthameApplication
            {
                IsRunningOnWin32 = Environment.OSVersion.Platform == PlatformID.Win32NT,
                IsWindowed = true,
                UserDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Athame"),
                CurrentWorkingDirectory = Directory.GetCurrentDirectory()
        }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        

        public static bool IsRunningOnWindows => 
    }
}
