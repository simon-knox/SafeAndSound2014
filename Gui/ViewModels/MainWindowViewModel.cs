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
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IBackupSetService _backupSetService;
        private IUIVisualizerService _uiVisualizerService;
        //private IMessageService _messageService;
        private IMessageBoxService _messageBoxService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IBackupSetService backupSetService, IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => backupSetService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageBoxService);

            _backupSetService = backupSetService;
            _uiVisualizerService = uiVisualizerService;
            _messageBoxService = messageBoxService;

            AddBackupSet = new Command(OnAddBackupSetExecute);
            EditBackupSet = new Command(OnEditBackupSetExecute, OnEditBackupSetCanExecute);
            RemoveBackupSet = new Command(OnRemoveBackupSetCollectionExecute, OnRemoveBackupSetCollectionCanExecute);
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected BackupSet.
        /// </summary>
        public BackupSetViewModel SelectedBackupSet
        {
            get { return GetValue<BackupSetViewModel>(SelectedBackupSetProperty); }
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
        public static readonly PropertyData SelectedBackupSetProperty = RegisterProperty("SelectedBackupSet", typeof(BackupSetViewModel), null);




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
        public ObservableCollection<BackupSetViewModel> BackupSets
        {
            get { return GetValue<ObservableCollection<BackupSetViewModel>>(BackupSetsProperty); }
            set { SetValue(BackupSetsProperty, value); }
        }

        /// <summary>
        /// Register the name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData BackupSetsProperty = RegisterProperty("BackupSets", typeof(ObservableCollection<BackupSetViewModel>), null);


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
            //var BackupSet = new BackupSetViewModel();
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the BackupSetViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var backupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(new BackupSet());



            //var BackupSetCollectionViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(BackupSetViewModel);
            if (_uiVisualizerService.ShowDialog(backupSetViewModel) ?? false)
            {
                BackupSets.Add(backupSetViewModel);
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
           // var typeFactory = this.GetTypeFactory();
            //var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<BackupSetViewModel>(SelectedBackupSet);
            _uiVisualizerService.ShowDialog(SelectedBackupSet);

            //var typeFactory = this.GetTypeFactory();
            //var BackupSetViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DriveSelectionViewModel>();
            //_uiVisualizerService.ShowDialog(BackupSetViewModel);
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
            

            //if (_messageService.Show(string.Format("Are you sure you want to delete the BackupSet '{0}'?", SelectedBackupSet),
             //   "Are you sure?", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            if (_messageBoxService.ShowMessage(string.Format("Are you sure you want to delete the BackupSet '{0}'?", SelectedBackupSet.Name), "Delete Backup Set", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            BackupSets = new ObservableCollection<BackupSetViewModel>(backupSets.Select(bs => new BackupSetViewModel(bs, _uiVisualizerService)));
        }

        protected override void OnClosing()
        {
            var backupSets = BackupSets.Select(bs => bs.BackupSet);
            _backupSetService.SaveBackupSets(backupSets);
            base.OnClosing();
        }

        #endregion
    }
}
