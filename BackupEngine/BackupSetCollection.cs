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
        #region public properties

        public static readonly PropertyData BackupSetsProperty = RegisterProperty("BackupSets", typeof(ObservableCollection<BackupSet>), () => new ObservableCollection<BackupSet>());

        ///<summary>
        /// The list of BackupSetCollection
        ///</summary>
        public ObservableCollection<BackupSet> BackupSets
        {
            get { return GetValue<ObservableCollection<BackupSet>>(BackupSetsProperty); }
            private set { SetValue(BackupSetsProperty, value); }
        }

        public static readonly PropertyData SaveTimeProperty = RegisterProperty("SaveTime", typeof(DateTime));

        ///<summary>
        /// The list of BackupSetCollection
        ///</summary>
        public DateTime SaveTime
        {
            get { return GetValue<DateTime>(SaveTimeProperty); }
            set { SetValue(SaveTimeProperty, value); }
        }       

        #endregion public properties


    }
}

