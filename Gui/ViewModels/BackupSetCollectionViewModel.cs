using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.BackupEngine;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class BackupSetCollectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetCollectionViewModel"/> class.
        /// </summary>
        public BackupSetCollectionViewModel(BackupSetCollection backupSetCollection)
        {
            Argument.IsNotNull(() => backupSetCollection);
            BackupSetCollection = backupSetCollection;
        }


        public static readonly PropertyData BackupSetCollectionProperty = RegisterProperty("BackupSetCollection", typeof(BackupSetCollection), null);
        [Model]
        public BackupSetCollection BackupSetCollection
        {
            get { return GetValue<BackupSetCollection>(BackupSetCollectionProperty); }
            private set { SetValue(BackupSetCollectionProperty, value); }
        }      
        
        public static readonly PropertyData BackupSetsProperty = RegisterProperty("BackupSets", typeof(ObservableCollection<BackupSet>), null); 
        /// <summary>
        /// Gets the family members.
        /// </summary>
        [ViewModelToModel("BackupSetCollection")]
        public ObservableCollection<BackupSet> BackupSets
        {
            get { return GetValue<ObservableCollection<BackupSet>>(BackupSetsProperty); }
            private set { SetValue(BackupSetsProperty, value); }
        }

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "View model title"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
    }
}
