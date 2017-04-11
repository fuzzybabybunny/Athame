using System;
using System.Media;
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
                Caption = "Athame",
                InstructionText = message ?? "Please wait...",
                StandardButtons = TaskDialogStandardButtons.Cancel,
                OwnerWindowHandle = owner?.Handle ?? IntPtr.Zero
            };

            var bar = new TaskDialogProgressBar { State = TaskDialogProgressBarState.Marquee };
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
                InstructionText = errorText ?? "An unknown error occurred",
                Text = "If you keep experiencing errors, you may need to sign in again. Click \"Show details\" for technical information.",
                DetailsExpandedLabel = "Hide details",
                DetailsCollapsedLabel = "Show details",
                DetailsExpandedText = exception.GetType().Name + ": " + exception.Message + "\n" + exception.StackTrace,
                ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
                StandardButtons = TaskDialogStandardButtons.Ok,
            };
            td.Opened += (sender, args) =>
            {
                td.Icon = TaskDialogStandardIcon.Error;
                SystemSounds.Hand.Play();
            };
            return td;
        }

        public static TaskDialog Error(IWin32Window owner, Exception exception, string errorText)
        {
            var td = Error(exception, errorText);
            td.OwnerWindowHandle = owner.Handle;
            return td;
        }

        public static TaskDialog Message(IWin32Window owner, TaskDialogStandardIcon icon, string title, string caption,
            string message, TaskDialogStandardButtons buttons)
        {
            var td = new TaskDialog
            {
                Icon = icon,
                Caption = title,
                InstructionText = caption,
                Text = message,
                StandardButtons = buttons,
                OwnerWindowHandle = owner.Handle
            };
            td.Opened += (sender, args) =>
            {
                td.Icon = icon;
                switch (icon)
                {
                    case TaskDialogStandardIcon.None:
                        break;
                    case TaskDialogStandardIcon.Warning:
                        SystemSounds.Exclamation.Play();
                        break;
                    case TaskDialogStandardIcon.Error:
                        SystemSounds.Hand.Play();
                        break;
                    case TaskDialogStandardIcon.Information:
                        SystemSounds.Asterisk.Play();
                        break;
                    case TaskDialogStandardIcon.Shield:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
                }
            };
            return td;
        }

    }
}
