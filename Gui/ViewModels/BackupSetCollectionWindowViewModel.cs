using Catel;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Services;
using Catel.IoC;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Collections.ObjectModel;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{


    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class BackupSetCollectionWindowViewModel : ViewModelBase
    {
        private IUIVisualizerService _uiVisualizerService;
        private IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetCollectionViewModel"/> class.
        /// </summary>
        public BackupSetCollectionWindowViewModel(BackupSetCollection backupSetCollection, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => backupSetCollection);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            BackupSetCollection = backupSetCollection;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            AddBackupSet = new Command(OnAddBackupSetExecute);
            EditBackupSet = new Command(OnEditBackupSetExecute, OnEditBackupSetCanExecute);
            RemoveBackupSet = new Command(OnRemoveBackupSetExecute, OnRemoveBackupSetCanExecute);
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

        public static readonly PropertyData SelectedBackupSetProperty = RegisterProperty("SelectedBackupSet", typeof(BackupSet), null);
        /// <summary>
        /// Gets or sets the selected BackupSet.
        /// </summary>
        public BackupSet SelectedBackupSet
        {
            get { return GetValue<BackupSet>(SelectedBackupSetProperty); }
            set { SetValue(SelectedBackupSetProperty, value); }
        }

        /// <summary>
        /// Gets the AddBackupSet command.
        /// </summary>
        public Command AddBackupSet { get; private set; }

        /// <summary>
        /// Method to invoke when the AddBackupSet command is executed.
        /// </summary>
        private void OnAddBackupSetExecute()
        {
            var BackupSet = new BackupSet();
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the BackupSetViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(BackupSet);
            if (_uiVisualizerService.ShowDialog(BackupSetViewModel) ?? false)
            {
                BackupSets.Add(BackupSet);
            }
        }

        /// <summary>
        /// Gets the EditBackupSet command.
        /// </summary>
        public Command EditBackupSet { get; private set; }

        /// <summary>
        /// Method to check whether the EditBackupSet command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnEditBackupSetCanExecute()
        {
            return SelectedBackupSet != null;
        }

        /// <summary>
        /// Method to invoke when the EditBackupSet command is executed.
        /// </summary>
        private void OnEditBackupSetExecute()
        {
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the BackupSetViewModel will add in the future
            //var typeFactory = this.GetTypeFactory();
            var typeFactory = this.GetTypeFactory();
            var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(SelectedBackupSet);
            _uiVisualizerService.ShowDialog(BackupSetViewModel);
        }

        /// <summary>
        /// Gets the RemoveBackupSet command.
        /// </summary>
        public Command RemoveBackupSet { get; private set; }

        /// <summary>
        /// Method to check whether the RemoveBackupSet command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveBackupSetCanExecute()
        {
            return SelectedBackupSet != null;
        }

        /// <summary>
        /// Method to invoke when the RemoveBackupSet command is executed.
        /// </summary>
        private void OnRemoveBackupSetExecute()
        {
            if (_messageService.Show(string.Format("Are you sure you want to delete the BackupSet '{0}'?", SelectedBackupSet),
                "Are you sure?", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                BackupSets.Remove(SelectedBackupSet);
                SelectedBackupSet = null;
            }
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
