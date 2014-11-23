using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Services;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using SKnoxConsulting.SafeAndSound.Gui.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class BackupSetViewModel : ViewModelBase
    {
        private IUIVisualizerService _uiVisualizerService;
        private ChangeNotificationWrapper _totalFileCountChanged;
        //private ChangeNotifyingObservableCollection<ActionLogItemViewModel> _actionLog2;
        private ICollectionView _actionLog2View;
        private ChangeNotifyingObservableCollection<ActionLogItemViewModel> _actionLog2;
        private Timer _timer;

        public BackupSetViewModel(BackupSet backupSet, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => backupSet);
            Argument.IsNotNull(() => uiVisualizerService);
            BackupSet = backupSet;
            _uiVisualizerService = uiVisualizerService;
            _timer = new Timer(new TimerCallback((o)=>
            {
                RefreshLog();
            }), null, Timeout.Infinite, Timeout.Infinite);

            BrowseSourceCommand = new Command(() => SourceDirectory = SetDirectory(SourceDirectory, "Select Source Directory"));
            BrowseDestinationCommand = new Command(() => DestinationDirectory = SetDirectory(DestinationDirectory, "Select Destination Directory"));
            ExcludeDirectoriesCommand = new Command(OnExcludeDirectoriesExecute, ()=>!String.IsNullOrEmpty(SourceDirectory));
            RunBackupCommand = new Command(() => 
                {
                    if(BackupSet.DestinationType == BackupDestinationType.ExternalDrive)
                    {
                        var typeFactory = this.GetTypeFactory();
                        var driveSelectionViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DriveSelectionViewModel>();
                        if(_uiVisualizerService.ShowDialog(driveSelectionViewModel) == true )
                        {
                            UpdateDestinationDriveLetter(driveSelectionViewModel.SelectedDrive.Name);
                        }
                        
                    }
                    _timer.Change(1000, 1000);
                    BackupSet.RunBackup();
                }  
                , () =>   ProcessingStatus == BackupProcessingStatus.NotStarted ||
                                                                                ProcessingStatus == BackupProcessingStatus.Cancelled ||
                                                                                ProcessingStatus == BackupProcessingStatus.Finished);
            CancelBackupCommand = new Command(() =>
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    BackupSet.CancelBackup();
                }
                , () => ProcessingStatus != BackupProcessingStatus.NotStarted &&
                                                                                    ProcessingStatus != BackupProcessingStatus.Cancelled &&
                                                                                    ProcessingStatus != BackupProcessingStatus.Finished);

            BackupSet.PropertyChanged += BackupSetPropertyChanged;            
        }

        private void RefreshLog()
        {
            if (ActionLog2 != null)
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {

                    //ActionLogView.Refresh();
                   //trac RaisePropertyChanged(() => ActionLogView);
                });
            }            
        }

        void BackupSetPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                {
                    RaisePropertyChanged(() => ShowBackupRunDetails);
                    RaisePropertyChanged(() => ActionLog2);                    
                    return;
                }
                case "ProcessingStatus":
                {
                    if (ProcessingStatus == BackupProcessingStatus.ActionQueueBuilt)
                    {
                        //Task.Factory.StartNew(() =>
                       // {
                        ActionLog2 = new ChangeNotifyingObservableCollection<ActionLogItemViewModel>(ActionLog.Select(ba => new ActionLogItemViewModel(ba)));
                        ActionLogView = (CollectionView)CollectionViewSource.GetDefaultView(ActionLog2);
                        ActionLogView.Filter = IsToBeShownInLog;
                        ActionLogView.Refresh();
                        RaisePropertyChanged(() => ActionLogView);

                           // ActionLog2 = new ChangeNotifyingObservableCollection<ActionLogItemViewModel>(ActionLog.Select(ba => new ActionLogItemViewModel(ba)));
                        //})
                       // .ContinueWith((t) =>
                        //{ 
                            RaisePropertyChanged(() => ActionLog2);
                        //}); 
                    }
                    return;
                }
            }
        }     
            
                    

        public static readonly PropertyData BackupSetProperty = RegisterProperty("BackupSet", typeof(BackupSet), null);
        [Model]
        public BackupSet BackupSet
        {
            get { return GetValue<BackupSet>(BackupSetProperty); }
            set { SetValue(BackupSetProperty, value); }
        }

        public static readonly PropertyData BackupModeProperty = RegisterProperty("BackupMode", typeof(BackupSetMode), BackupSetMode.Backup);
        ///<summary>
        /// The Mode that the backup set is is e.g. Backup or Restore
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public BackupSetMode BackupMode
        {
            get { return GetValue<BackupSetMode>(BackupModeProperty); }
            set { SetValue(BackupModeProperty, value); }
        }

        public static readonly PropertyData ActionLogProperty = RegisterProperty("ActionLog", typeof(IEnumerable<BackupAction>), () => new List<BackupAction>());
        [ViewModelToModel("BackupSet")]
        public IEnumerable<BackupAction> ActionLog
        {
            get { return GetValue<IEnumerable<BackupAction>>(ActionLogProperty); }
        }


        public CollectionView ActionLogView
        { get; private set; }



        public ChangeNotifyingObservableCollection<ActionLogItemViewModel> ActionLog2
        {
            get
            {
               // return _actionLog2
                   // .Where(a => (a.Status == ActionStatus.Pending.ToString() && ShowPendingInLog) ||
                   //                             (a.Status == ActionStatus.Complete.ToString() && ShowCompleteInLog) ||
                   //                             (a.Status == ActionStatus.Skipped.ToString() && ShowSkippedInLog) ||
                   //                             (a.Status == ActionStatus.Failed.ToString() && ShowFailedInLog));   
                //if (_actionLog2 == null)
                //{
                //    _actionLog2 = new ObservableCollection<ActionLogItemViewModel>();

                //    _actionLog2View = CollectionViewSource.GetDefaultView(_actionLog2);
                //    _actionLog2View.Filter = IsToBeShownInLog;
                //}
                return _actionLog2;

            }
            private set
            { 
                _actionLog2 = value;
                RaisePropertyChanged(() => ActionLog2);
            }
           
        }

        private bool IsToBeShownInLog(object obj)
        {
            var item = obj as ActionLogItemViewModel;
            if(item != null)
            {
                return (item.Status == ActionStatus.Pending.ToString() && ShowPendingInLog) ||
                    (item.Status == ActionStatus.Complete.ToString() && ShowCompleteInLog) ||
                    (item.Status == ActionStatus.Skipped.ToString() && ShowSkippedInLog) ||
                    (item.Status == ActionStatus.Failed.ToString() && ShowFailedInLog); 
            }
            return false;
        }

        //private async Task<IEnumerable<ActionLogItemViewModel>> FilterActionLog()
        //{
        //    IEnumerable<ActionLogItemViewModel> actionLog2 = null;

        //    await Task.Run(() => actionLog2 = ActionLog.Where(a => (a.Status == ActionStatus.Pending && ShowPendingInLog) ||
        //                                        (a.Status == ActionStatus.Complete && ShowCompleteInLog) ||
        //                                        (a.Status == ActionStatus.Skipped && ShowSkippedInLog) ||
        //                                        (a.Status == ActionStatus.Failed && ShowFailedInLog))
        //                          .Select(a => new ActionLogItemViewModel(a)));
        //    return;
        //}

        

        /////<summary>
        ///// The list of BackupErrors
        /////</summary>
        //public IEnumerable<BackupError> ErrorLog
        //{
        //    get
        //    {
        //        return ActionQueue.Where(a=>a.Status == ActionStatus.Failed)
        //            .Select(a=>new BackupError(a.))
        //            .Union(DeleteActionStack.Where(da=>da.Status == ActionStatus.Failed))
        //            .Select(ca=>new BackupError())



        //        var result = new List<BackupError>();
        //        foreach (BackupAction ba in _actionQueue)
        //        {
        //            if (ba.Status == ActionStatus.Failed)
        //            {
        //                result.Add(new BackupError("Test", "Dest", null));
        //            }
        //        }
        //        foreach (BackupAction ba in _deleteStack)
        //        {
        //            if (ba.Status == ActionStatus.Failed)
        //            {
        //                result.Add(new BackupError("Test", "Dest", null));
        //            }
        //        }
        //        return result;
        //    }
        //}

        public static readonly PropertyData ExcludedDirectoriesProperty = RegisterProperty("ExcludedDirectories", typeof(HashSet<string>), () => new HashSet<string>());
        ///<summary>
        /// The set of excluded directories from the BackupSet
        ///</summary>        
        [ViewModelToModel("BackupSet")]
        public HashSet<string> ExcludedDirectories
        {
            get { return GetValue<HashSet<string>>(ExcludedDirectoriesProperty); }
            set { SetValue(ExcludedDirectoriesProperty, value); }
        }


        public static readonly PropertyData DestinationDirectoryProperty = RegisterProperty("DestinationDirectory", typeof(string), string.Empty);
        ///<summary>
        /// The destination directory of the BackupSet
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public string DestinationDirectory
        {
            get { return GetValue<string>(DestinationDirectoryProperty); }
            set { SetValue(DestinationDirectoryProperty, value); }
        }        

        public static readonly PropertyData DestinationTypeProperty = RegisterProperty("DestinationType", typeof(BackupDestinationType));
        ///<summary>
        /// The destination directory of the BackupSet
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public BackupDestinationType DestinationType
        {
            get { return GetValue<BackupDestinationType>(DestinationTypeProperty); }
            set { SetValue(DestinationTypeProperty, value); }
        }

        public static readonly PropertyData SourceDirectoryProperty = RegisterProperty("SourceDirectory", typeof(string), string.Empty);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public string SourceDirectory
        {
            get { return GetValue<string>(SourceDirectoryProperty); }
            set { SetValue(SourceDirectoryProperty, value); }
        }

        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty);
        /// <summary>
        /// The name of the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly PropertyData IncludeReadOnlyProperty = RegisterProperty("IncludeReadOnly", typeof(bool), true);
        /// <summary>
        /// Should read only files be included in the backup
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public bool IncludeReadOnly
        {
            get { return GetValue<bool>(IncludeReadOnlyProperty); }
            set { SetValue(IncludeReadOnlyProperty, value); }
        }

        public static readonly PropertyData IncludeHiddenProperty = RegisterProperty("IncludeHidden", typeof(bool), true);
        /// <summary>
        /// Should hidden files be included in the backup
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public bool IncludeHidden
        {
            get { return GetValue<bool>(IncludeHiddenProperty); }
            set { SetValue(IncludeHiddenProperty, value); }
        }

        public static readonly PropertyData IncludeSystemProperty = RegisterProperty("IncludeSystem", typeof(bool), true);
        /// <summary>
        /// Should system files be included in the backup
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public bool IncludeSystem
        {
            get { return GetValue<bool>(IncludeSystemProperty); }
            set { SetValue(IncludeSystemProperty, value); }
        }

        public static readonly PropertyData RemoveDeletedProperty = RegisterProperty("RemoveDeleted", typeof(bool), false);
        /// <summary>
        /// Should files and folders that are in the destination but not the source be deleted
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public bool RemoveDeleted
        {
            get { return GetValue<bool>(RemoveDeletedProperty); }
            set { SetValue(RemoveDeletedProperty, value); }
        }

        public static readonly PropertyData ProcessingStatusProperty = RegisterProperty("ProcessingStatus", typeof(BackupProcessingStatus), BackupProcessingStatus.NotStarted);
        /// <summary>
        /// The status of the backup processing
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public BackupProcessingStatus ProcessingStatus
        {
            get { return GetValue<BackupProcessingStatus>(ProcessingStatusProperty); }
            set 
            { 
                SetValue(ProcessingStatusProperty, value);
                RaisePropertyChanged(() => ShowBackupRunDetails);
            }
        }

        public static readonly PropertyData StatusProperty = RegisterProperty("Status", typeof(string));
        /// <summary>
        /// The status of the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public string Status
        {
            get
            {
                return GetValue<string>(StatusProperty);
            }            
            private set 
            {
                SetValue(StatusProperty, value);
                RaisePropertyChanged(() => ShowBackupRunDetails);
            }
        }

        [ExcludeFromValidation]
        public bool ShowBackupRunDetails
        {
            get { return Status != "Ready."; }
        }

        public static readonly PropertyData TotalFileCountProperty = RegisterProperty("TotalFileCount", typeof(int), 0);
        /// <summary>
        /// The total number of files in the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public int TotalFileCount
        {
            get { return GetValue<int>(TotalFileCountProperty); }
            private set { SetValue(TotalFileCountProperty, value); }
        }

        public static readonly PropertyData TotalFileCountMaximumProperty = RegisterProperty("TotalFileCountMaximum", typeof(int), 0);
        /// <summary>
        /// The total number of files to use as a maximum value
        /// </summary> 
        [ViewModelToModel("BackupSet")]
        public int TotalFileCountMaximum
        {
            //get { return TotalFileCount == 0 ? 1 : TotalFileCount; }
            get { return GetValue<int>(TotalFileCountMaximumProperty); }
           // private set { SetValue(TotalFileCountMaximumProperty, value); }
        }

        public static readonly PropertyData FolderDeleteCountProperty = RegisterProperty("FolderDeleteCount", typeof(int), 0);
        /// <summary>
        /// The number of folders that have been deleted
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public int FolderDeleteCount
        {
            get { return GetValue<int>(FolderDeleteCountProperty); }
            private set { SetValue(FolderDeleteCountProperty, value); }
        }

        public static readonly PropertyData FileDeleteCountProperty = RegisterProperty("FileDeleteCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been deleted
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public int FileDeleteCount
        {
            get { return GetValue<int>(FileDeleteCountProperty); }
            private set { SetValue(FileDeleteCountProperty, value); }
        }

        public static readonly PropertyData FileCopyCountProperty = RegisterProperty("FileCopyCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been copied
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public int FileCopyCount
        {
            get { return GetValue<int>(FileCopyCountProperty); }
            private set { SetValue(FileCopyCountProperty, value); }
        }

        public static readonly PropertyData FolderCreateCountProperty = RegisterProperty("FolderCreateCount", typeof(int), 0);
        ///<summary>
        /// The number of folders that have been created
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public int FolderCreateCount
        {
            get { return GetValue<int>(FolderCreateCountProperty); }
            private set { SetValue(FolderCreateCountProperty, value); }
        }

        public static readonly PropertyData FileOverwriteCountProperty = RegisterProperty("FileOverwriteCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been overwritten
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public int FileOverwriteCount
        {
            get { return GetValue<int>(FileOverwriteCountProperty); }
            private set { SetValue(FileOverwriteCountProperty, value); }
        }

        public static readonly PropertyData FileSkipCountProperty = RegisterProperty("FileSkipCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been skipped
        ///</summary>
        [ViewModelToModel("BackupSet")]
        public int FileSkipCount
        {
            get { return GetValue<int>(FileSkipCountProperty); }
            private set { SetValue(FileSkipCountProperty, value); }
        }

        public static readonly PropertyData ErrorCountProperty = RegisterProperty("ErrorCount", typeof(int), 0);
        /// <summary>
        /// The number of errors that have occured
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public int ErrorCount
        {
            get { return GetValue<int>(ErrorCountProperty); }
            private set { SetValue(ErrorCountProperty, value); }
        }

        public static readonly PropertyData ProcessingProgressCountProperty = RegisterProperty("ProcessingProgressCount", typeof(int), 0);
        /// <summary>
        /// The number of BackupActions that have been processed
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public int ProcessingProgressCount
        {
            get { return FileCopyCount + FileOverwriteCount + FileSkipCount + ErrorCount; }
        }

        public static readonly PropertyData ActionQueueProperty = RegisterProperty("ActionQueue", typeof(Queue<BackupAction>), new Queue<BackupAction>());
        /// <summary>
        /// The queue of BackupActions
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public Queue<BackupAction> ActionQueue
        {
            get { return GetValue<Queue<BackupAction>>(ActionQueueProperty); }
            private set { SetValue(ActionQueueProperty, value); }
        }

        public static readonly PropertyData SkipFileActionQueueProperty = RegisterProperty("SkipFileActionQueue", typeof(Queue<BackupAction>), new Queue<BackupAction>());
        /// <summary>
        /// The queue of BackupActions that will be skipped
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public Queue<BackupAction> SkipFileActionQueue
        {
            get { return GetValue<Queue<BackupAction>>(SkipFileActionQueueProperty); }
            private set { SetValue(SkipFileActionQueueProperty, value); }
        }

        public static readonly PropertyData DeleteActionStackProperty = RegisterProperty("DeleteActionStack", typeof(Stack<BackupAction>), new Stack<BackupAction>());
        /// <summary>
        /// The stack of Delete BackupActions
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public Stack<BackupAction> DeleteActionStack
        {
            get { return GetValue<Stack<BackupAction>>(DeleteActionStackProperty); }
            private set { SetValue(DeleteActionStackProperty, value); }
        }

        public static readonly PropertyData ShowPendingInLogProperty = RegisterProperty("ShowPendingInLog", typeof(bool),true);

        public bool ShowPendingInLog
        {
            get { return GetValue<bool>(ShowPendingInLogProperty); }
            set 
            { 
                SetValue(ShowPendingInLogProperty, value);
                RaisePropertyChanged(() => ActionLog2);
            }
        }

        public static readonly PropertyData ShowCompleteInLogProperty = RegisterProperty("ShowCompleteInLog", typeof(bool),true);

        public bool ShowCompleteInLog
        {
            get { return GetValue<bool>(ShowCompleteInLogProperty); }
            set 
            { 
                SetValue(ShowCompleteInLogProperty, value); 
                RaisePropertyChanged(() => ActionLog2); 
            }
        }

        public static readonly PropertyData ShowSkippedInLogProperty = RegisterProperty("ShowSkippedInLog", typeof(bool),true);

        public bool ShowSkippedInLog
        {
            get { return GetValue<bool>(ShowSkippedInLogProperty); }
            set 
            { 
                SetValue(ShowSkippedInLogProperty, value);
                ActionLogView.Refresh();
                RaisePropertyChanged(() => ActionLog2); 
            }
        }

        public static readonly PropertyData ShowFailedInLogProperty = RegisterProperty("ShowFailedInLog", typeof(bool),true);

        public bool ShowFailedInLog
        {
            get { return GetValue<bool>(ShowFailedInLogProperty); }
            set 
            { 
                SetValue(ShowFailedInLogProperty, value);
                ActionLogView.Refresh();  
                RaisePropertyChanged(() => ActionLog2);
            }
        }   

        public ICommand BrowseSourceCommand
        { get; private set; }

        public ICommand BrowseDestinationCommand
        { get; private set; }

        public ICommand ExcludeDirectoriesCommand
        { get; private set; }

        public ICommand RunBackupCommand
        { get; private set; }

        public ICommand CancelBackupCommand
        { get; private set; }

        /// <summary>
        /// Method to invoke when the AddBackupSet command is executed.
        /// </summary>
        private void OnExcludeDirectoriesExecute()
        {
            //ExcludedDirectories
            //// Note that we use the type factory here because it will automatically take care of any dependencies
            //// that the BackupSetViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var excludedFilesViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ExcludedFilesViewModel>(BackupSet);

            if (_uiVisualizerService.ShowDialog(excludedFilesViewModel) ?? false)
            {
                var v = excludedFilesViewModel.Items[0].GetExcludedDirectories();


                int h = 7;//BackupSets.Add(BackupSet);
            }       
            int i = 0;
        }

        private string SetDirectory(string directory, string title="Select Directory")
        {            
            var dependencyResolver = this.GetDependencyResolver();
            var selectDirectoryService = dependencyResolver.Resolve<ISelectDirectoryService>();
            selectDirectoryService.Title = title;
            if (!string.IsNullOrEmpty(directory))
            {
                selectDirectoryService.InitialDirectory = directory;
            }
            
            if (selectDirectoryService.DetermineDirectory())
            {
                return selectDirectoryService.DirectoryName;
            }
            return directory;
        }

        private void UpdateDestinationDriveLetter(string driveName)
        {
            if (!string.IsNullOrWhiteSpace(driveName))
            {
                DestinationDirectory = driveName.Substring(0,1) +  DestinationDirectory.Substring(DestinationDirectory.IndexOf(':'));
            }
        }

       

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
