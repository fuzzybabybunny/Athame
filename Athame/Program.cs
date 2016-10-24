using System;
using System.Windows.Forms;
using Athame.UI;

namespace Athame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static bool IsRunningOnWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
