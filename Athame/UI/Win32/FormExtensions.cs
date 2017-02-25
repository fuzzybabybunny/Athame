using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.UI.Win32;

namespace Athame.UI.Win32
{
    public static class FormExtensions
    {
        [DllImport(Native.User32, CharSet = CharSet.Auto, SetLastError = false)]
        private static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public static void DragMove(this Form form)
        {
            ReleaseCapture();
            Native.SendMessage(form.Handle, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
        }
    }
}
