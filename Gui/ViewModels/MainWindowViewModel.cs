namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using SKnoxConsulting.SafeAndSound.BackupEngine;
    using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
    using System.Collections.ObjectModel;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IBackupSetService _backupSetService;
        private IUIVisualizerService _uiVisualizerService;
        private IMessageService _messageService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IBackupSetService backupSetService, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => backupSetService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            _backupSetService = backupSetService;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            AddBackupSet = new Command(OnAddBackupSetExecute);
            EditBackupSet = new Command(OnEditBackupSetExecute, OnEditBackupSetCanExecute);
            RemoveBackupSet = new Command(OnRemoveBackupSetCollectionExecute, OnRemoveBackupSetCollectionCanExecute);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected BackupSet.
        /// </summary>
        public BackupSet SelectedBackupSet
        {
            get { return GetValue<BackupSet>(SelectedBackupSetProperty); }
            set 
            { 
                SetValue(SelectedBackupSetProperty, value); 
                RaisePropertyChanged(() => IsBackupSetSelected);
            }
        }

        public bool IsBackupSetSelected
        {
            get { return SelectedBackupSet != null; }
        }            

        /// <summary>
        /// Register the SelectedBackupSet property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedBackupSetProperty = RegisterProperty("SelectedBackupSet", typeof(BackupSet), null);




        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "Safe and Sound Backup 2014"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<BackupSet> BackupSets
        {
            get { return GetValue<ObservableCollection<BackupSet>>(BackupSetsProperty); }
            set { SetValue(BackupSetsProperty, value); }
        }

        /// <summary>
        /// Register the name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData BackupSetsProperty = RegisterProperty("BackupSets", typeof(ObservableCollection<BackupSet>), null);


        #endregion

        #region Commands
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
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
            var BackupSetCollectionViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(BackupSet);
            if (_uiVisualizerService.ShowDialog(BackupSetCollectionViewModel) ?? false)
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
            //var typeFactory = this.GetTypeFactory();
           // var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(SelectedBackupSet);
            //_uiVisualizerService.ShowDialog(BackupSetViewModel);

            var typeFactory = this.GetTypeFactory();
            var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DriveSelectionViewModel>();
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
        private bool OnRemoveBackupSetCollectionCanExecute()
        {
            return SelectedBackupSet != null;
        }

        /// <summary>
        /// Method to invoke when the RemoveBackupSet command is executed.
        /// </summary>
        private void OnRemoveBackupSetCollectionExecute()
        {
            if (_messageService.Show(string.Format("Are you sure you want to delete the BackupSet '{0}'?", SelectedBackupSet),
                "Are you sure?", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                BackupSets.Remove(SelectedBackupSet);
                SelectedBackupSet = null;
            }
        }
        #endregion

        #region Methods
        // TODO: Create your methods here

        protected override void Initialize()
        {
            var backupSets = _backupSetService.LoadBackupSets();
            BackupSets = new ObservableCollection<BackupSet>(backupSets);
        }

        protected override void Close()
        {
            
            _backupSetService.SaveBackupSets(BackupSets);
        }

        #endregion
    }
}
