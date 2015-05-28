using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Services
{
    public static class StateService
    {
        static StateService()
        {

        }

        public static void RequestBackupSetEdit(string id)
        {
            if (BackupSetViewFlipRequested != null)
                BackupSetViewFlipRequested(id);
        } 


        //public static event EventHandler BackupSetViewFlipRequested;

        public static event FlipRequestedEventHandler BackupSetViewFlipRequested;


    }

    public delegate void FlipRequestedEventHandler(string id);

    public class FlipControlEventArgs : EventArgs
    {
        public FlipControlEventArgs(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
