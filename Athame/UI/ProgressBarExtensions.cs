using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
// ReSharper disable InconsistentNaming

namespace Athame.UI
{
    /// <summary>
    /// Progress bar state as defined in CommCtrl.h.
    /// </summary>
    public enum ProgressBarState
    {
        /// <summary>
        /// Progress bar appears green (default)
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Progress bar appears red
        /// </summary>
        Error = 2,
        /// <summary>
        /// Progress bar appears yellow
        /// </summary>
        Warning = 3
    }

    public static class ProgressBarExtensions
    {
        private const int WM_USER = 0x400;

        // CommCtrl.h
        private const int PBM_SETSTATE = WM_USER + 16;
        private const int PBM_GETSTATE = WM_USER + 17;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public static ProgressBarState GetState(this ProgressBar progressBar)
        {
            return (ProgressBarState)SendMessage(progressBar.Handle, PBM_GETSTATE, IntPtr.Zero, IntPtr.Zero);
        }

        public static void SetState(this ProgressBar progressBar, ProgressBarState state)
        {
            SendMessage(progressBar.Handle, PBM_SETSTATE, new IntPtr((int) state), IntPtr.Zero);
        }
    }
}
