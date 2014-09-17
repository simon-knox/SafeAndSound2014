using System;
using System.IO;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.Events
{
    /// <summary>
    /// The event args for a drive being unavaliable
    /// </summary>
    public class DriveUnavailableEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driveLetter">The drive letter</param>
        /// <param name="driveType">The type of drive</param>
        public DriveUnavailableEventArgs(string driveLetter, DriveType driveType)
        {
            DriveLetter = driveLetter;
            TypeOfDrive = driveType;
        }

        /// <summary>
        /// The drive letter
        /// </summary>
        public string DriveLetter { get; private set; }

        /// <summary>
        /// The type of drive
        /// </summary>
        public DriveType TypeOfDrive { get; private set; }
    }
}
