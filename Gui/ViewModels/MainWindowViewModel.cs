using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Services;
using log4net;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using SKnoxConsulting.SafeAndSound.Gui.Services;
using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{  

    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);


    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private const string OVERDUE = "OVERDUE";
        private const string ERROR = "ERROR";

        #region Fields

        private readonly IBackupSetService _backupSetService;
        private IUIVisualizerService _uiVisualizerService;
        private IMessageBoxService _messageBoxService; 

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            _log.Info("In MainWindowViewModel constructor");

            _backupSetService = backupSetService;
            _uiVisualizerService = uiVisualizerService;
            _messageBoxService = messageBoxService;

            ServiceSettings = new ServiceViewModel();

            Themes = new[] { "Dark", "Light" };
            CurrentThemeNumber = 0;

            AddBackupSet = new Command(OnAddBackupSetExecute);
            EditBackupSet = new Command(OnEditBackupSetExecute, OnEditBackupSetCanExecute);
            RemoveBackupSet = new Command(OnRemoveBackupSetCollectionExecute, OnRemoveBackupSetCollectionCanExecute);

            OpenLogDirectoryCommand = new Command(OnShowLogDirectoryCommand);
            ShowAboutDialogCommand = new Command(() => _uiVisualizerService.ShowDialog(new AboutViewModel()));
            ToggleThemeCommand = new Command(() =>
                                                    {
                                                        CurrentThemeNumber++;
                                                        if(CurrentThemeNumber > Themes.Length - 1)
                                                        {
                                                            CurrentThemeNumber = 0;
                                                        }
                                                        RaiseThemeChanged(Themes[CurrentThemeNumber]);
                                                    });
            FilterAllBackupsCommand = new Command(() => FilterBackupSets());
            FilterOverdueBackupsCommand = new Command(() => FilterBackupSets(OVERDUE));
            FilterErrorBackupsCommand = new Command(() => FilterBackupSets(ERROR));

        

            Initialize();
        }

        #endregion

        public event ThemeChangedEventHandler OnThemeChanged;
        
        public ICommand OpenLogDirectoryCommand
        { get; private set; }

        public ICommand ShowAboutDialogCommand
        { get; private set; }

        public ICommand ToggleThemeCommand
        { get; private set; }

        public ICommand FilterAllBackupsCommand
        { get; private set; }

        public ICommand FilterOverdueBackupsCommand
        { get; private set; }

        public ICommand FilterErrorBackupsCommand
        { get; private set; }

        #region Properties

        public int CurrentThemeNumber
        { get; private set; }

        public string[] Themes
        { get; private set; }

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

        public ICollectionView BackupSetsView
        { get; private set; }

        /// <summary>
        /// Register the SelectedBackupSet property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedBackupSetProperty = RegisterProperty("SelectedBackupSet", typeof(BackupSetViewModel), null);




        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "Safe and Sound Backup 2015"; } }

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


        public ServiceViewModel ServiceSettings
        { get; set; }


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

        private void FilterBackupSets(string filter = "")
        {
            BackupSetsView = (CollectionView)CollectionViewSource.GetDefaultView(BackupSets);
            switch(filter)
            {
                case ERROR:
                    BackupSetsView.Filter = BackupSetsErrorFilter;
                    break;
                case OVERDUE:
                    BackupSetsView.Filter = BackupSetsOverdueFilter;
                    break;
                default:
                    BackupSetsView.Filter = BackupSetsNoFilter;
                    break;
            }      
            BackupSetsView.Refresh();
            RaisePropertyChanged(() => BackupSetsView);
        }

        private bool BackupSetsOverdueFilter(object obj)
        {
            var item = obj as BackupSetViewModel;
            if (item != null)
            {
                return (item.ScheduleStatus == Models.BackupSetScheduleStatus.NeverRun ||
                    item.ScheduleStatus == Models.BackupSetScheduleStatus.Overdue );
            }
            return false;
        }

        private bool BackupSetsNoFilter(object obj)
        {
            var item = obj as BackupSetViewModel;
            if (item != null)
            {
                return true;
            }
            return false;
        }

        private bool BackupSetsErrorFilter(object obj)
        {
            var item = obj as BackupSetViewModel;
            if (item != null)
            {
                return (item.ScheduleStatus == Models.BackupSetScheduleStatus.LastRunHadErrors);
            }
            return false;
        }

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

        private void OnShowLogDirectoryCommand()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //_sourceDirectory = Path.Combine(location, "CopyFileActionTestsSource");
            Process.Start(location);
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
            BackupSets = new ObservableCollection<BackupSetViewModel>(backupSets.OrderBy(b=>b.Name)
                .Select(bs => new BackupSetViewModel(bs, _uiVisualizerService)));
            FilterBackupSets();
        }

        protected override void OnClosing()
        {
            var backupSets = BackupSets.Select(bs => bs.BackupSet);
            _backupSetService.SaveBackupSets(backupSets);
            base.OnClosing();
        }

        private void RaiseThemeChanged(string themeName)
        {
            if(OnThemeChanged != null)
            {
                OnThemeChanged(this, new ThemeChangedEventArgs(themeName));
            }
        }

        #endregion
    }

    public class ThemeChangedEventArgs
    {
        public ThemeChangedEventArgs(string themeName)
        {
            ThemeName = themeName;
        }

        public string ThemeName
        {
            get;
            private set;
        }
    }
}
