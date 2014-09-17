using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Catel.Data;
using System.Collections.ObjectModel;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    ///<summary>
    /// A collection of BackupSet
    ///</summary>
    [Serializable]
    public class BackupSetCollection : SavableModelBase<BackupSetCollection>
    {
        #region private members

        //private FileSystemWatcher _fileSystemWatcher;

        #endregion private members

        #region constructor

        /////<summary>
        ///// Constructor for BackupSetCollection
        /////</summary>
        //public BackupSetCollection()
        //{
        //    if (!Directory.Exists(GlobalSettings.ApplicationDataPath))
        //    {
        //        Directory.CreateDirectory(GlobalSettings.ApplicationDataPath);
        //    }
        //    BackupSets = new List<BackupSet>();

        //    _fileSystemWatcher = new FileSystemWatcher(GlobalSettings.ApplicationDataPath, "*.bus");
        //    _fileSystemWatcher.EnableRaisingEvents = true;
        //    _fileSystemWatcher.Changed += ((o,e) => LoadBackupSets());

        //    LoadBackupSets();
        //}        

        #endregion constructor

        #region public properties

        public static readonly PropertyData BackupSetsProperty = RegisterProperty("BackupSets", typeof(ObservableCollection<BackupSet>), () => new ObservableCollection<BackupSet>());

        /////<summary>
        ///// Indexer for list of BackupSetCollection
        /////</summary>
        /////<param name="index"></param>
        //public BackupSet this[int index]
        //{
        //    get { return BackupSets != null ? BackupSets[index] : null; }
        //}

        ///<summary>
        /// The list of BackupSetCollection
        ///</summary>
        public ObservableCollection<BackupSet> BackupSets
        {
            get { return GetValue<ObservableCollection<BackupSet>>(BackupSetsProperty); }
            private set { SetValue(BackupSetsProperty, value); }
        }

        public static readonly PropertyData SaveTimeProperty = RegisterProperty("SaveTime", typeof(DateTime));

        /////<summary>
        ///// Indexer for list of BackupSetCollection
        /////</summary>
        /////<param name="index"></param>
        //public BackupSet this[int index]
        //{
        //    get { return BackupSets != null ? BackupSets[index] : null; }
        //}

        ///<summary>
        /// The list of BackupSetCollection
        ///</summary>
        public DateTime SaveTime
        {
            get { return GetValue<DateTime>(SaveTimeProperty); }
            set { SetValue(SaveTimeProperty, value); }
        }



        ///<summary>
        /// The number of BackupSetCollection in the BackupSetCollection
        ///</summary>
        //public new int Count
        // {
        //     get { return BackupSets != null ? BackupSets.Count : 0; }
        // }        

        #endregion public properties

        #region private methods

        //private void LoadBackupSets()
        //{
        //    BackupSets.Clear();
        //    var di = new DirectoryInfo(GlobalSettings.ApplicationDataPath);
        //    if (di.Exists)
        //    {
        //        BackupSets.AddRange(di.GetFiles("*.bus")
        //            .Select(f => OpenBackupSet(f.FullName)));
        //    }
        //}

        //private void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        //{
        //    LoadBackupSets();
        //}

        //private void OnBackupSetAdded(EventArgs e)
        //{
        //    if (BackupSetAdded != null)
        //    {
        //        BackupSetAdded(e);
        //    }
        //}

        //private void OnBackupSetRemoved(EventArgs e)
        //{
        //    if (BackupSetRemoved == null)
        //        return;
        //    BackupSetRemoved(e);
        //    CurrentIndex = -1;
        //}

        //private void OnCurrentBackupSetChanged(CurrentBackupSetChangedEventArgs e)
        //{
        //    if (CurrentBackupSetChanged != null)
        //    {
        //        CurrentBackupSetChanged(e);
        //    }
        //}

        //private static BackupSet OpenBackupSet(string name)
        //{
        //    //Open the file written above and read values from it.
        //    Stream stream = File.Open(name, FileMode.Open);
        //    stream.Seek(0, 0);
        //    var bformatter = new BinaryFormatter();
        //    var bs = (BackupSet) bformatter.Deserialize(stream);
        //    stream.Close();
        //    return bs;
        //}

        #endregion private methods
    }
}

