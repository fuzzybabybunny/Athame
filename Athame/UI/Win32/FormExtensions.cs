using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.UI.Win32;

namespace Athame.UI.Win32
{
    [Flags]
    public enum FlashMethod : uint
    {
        Stop = 0x0,
        Caption = 0x1,
        Tray = 0x2,
        All = Caption | Tray,
        Timer = 0x04,
        TimerNoForeground = 0xC
    }

    public static class FormExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        [DllImport(Native.User32)]
        private static extern bool ReleaseCapture();

        [DllImport(Native.User32)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public static void DragMove(this Form form)
        {
            ReleaseCapture();
            Native.SendMessage(form.Handle, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
        }

        public static void Flash(this Form form, FlashMethod method, int count, int rate)
        {
            var fInfo = new FLASHWINFO
            {
                hwnd = form.Handle,
                dwFlags = (uint)method,
                uCount = (uint)count,
                dwTimeout = (uint)rate
            };
            fInfo.cbSize = (uint)Marshal.SizeOf(fInfo);
            if (!FlashWindowEx(ref fInfo))
            {
                throw new Win32Exception();
            }
        }


    }
}
