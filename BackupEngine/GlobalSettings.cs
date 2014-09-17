using System;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    ///<summary>
    /// Contains settings global to the application
    ///</summary>
    public class GlobalSettings
    {
        /// <summary>
        /// The path that the applications data is found at
        /// </summary>
        public static string ApplicationDataPath
        {
            get
            {
                return string.Format("{0}{1}",
                                     Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                     "\\SKnoxConsulting\\SafeAndSoundBackup");
            }
        }
    }
}
