using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Services
{
    public static class DialogService
    {
        public static event MetroMessageBoxRequestEventHandler DialogRequested;

        public static void RequestDialog(string title, string message, MetroDialogSettings dialogSettings = null)
        {
            if (DialogRequested != null)
                DialogRequested(null, new MetroMessageBoxEventArgs(title, message, dialogSettings));
        }
    }

    public delegate void MetroMessageBoxRequestEventHandler(object sender, MetroMessageBoxEventArgs e);

    public class MetroMessageBoxEventArgs : EventArgs
    {
        public MetroMessageBoxEventArgs(string title, string message, MetroDialogSettings dialogSettings = null)
        {
            Title = title;
            Message = message;
            DialogSettings = dialogSettings;
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public MetroDialogSettings DialogSettings { get; set; }
    }
}
