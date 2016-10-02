using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Athame.UI
{
    public static class CommonTaskDialogs
    {

        public static TaskDialog Wait(IWin32Window owner, string message)
        {
            var dialog = new TaskDialog
            {
                Cancelable = false,
                Caption = "Please wait...",
                InstructionText = message ?? "Please wait...",
                StandardButtons = TaskDialogStandardButtons.Cancel,
                OwnerWindowHandle = owner != null ? owner.Handle : IntPtr.Zero
            };

            var bar = new TaskDialogProgressBar {State = TaskDialogProgressBarState.Marquee};
            dialog.ProgressBar = bar;
            return dialog;
        }

        public static TaskDialog Error(Exception exception, string errorText)
        {
            var td = new TaskDialog
            {
                DetailsExpanded = false,
                Cancelable = true,
                Icon = TaskDialogStandardIcon.Error,
                Caption = "Something went wrong!",
                InstructionText = errorText ?? "An unknown error occurred.",
                Text = exception.GetType().Name + ": " + exception.Message,
                DetailsExpandedLabel = "Hide details",
                DetailsCollapsedLabel = "Show details",
                DetailsExpandedText = exception.StackTrace,
                ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
                StandardButtons = TaskDialogStandardButtons.Ok
            };
            td.Opened += (sender, args) =>
            {
                td.Icon = TaskDialogStandardIcon.Error;
                SystemSounds.Hand.Play();
            };
            return td;
        }

    }
}
