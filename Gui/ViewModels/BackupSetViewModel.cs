using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Services;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class BackupSetViewModel : ViewModelBase
    {
        private IUIVisualizerService _uiVisualizerService;

        public BackupSetViewModel(BackupSet backupSet, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => backupSet);
            Argument.IsNotNull(() => uiVisualizerService);
            BackupSet = backupSet;
            _uiVisualizerService = uiVisualizerService;

            BrowseSourceCommand = new Command(() => SourceDirectory = SetDirectory(SourceDirectory, "Select Source Directory"));
            BrowseDestinationCommand = new Command(() => DestinationDirectory = SetDirectory(DestinationDirectory, "Select Destination Directory"));

            ExcludeDirectoriesCommand = new Command(OnExcludeDirectoriesExecute, ()=>!String.IsNullOrEmpty(SourceDirectory));
            
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
            set { SetValue(ProcessingStatusProperty, value); }
        }

        public static readonly PropertyData StatusProperty = RegisterProperty("Status", typeof(string));
        /// <summary>
        /// The status of the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public string Status
        {
            get { return GetValue<string>(StatusProperty); }            
            private set { SetValue(StatusProperty, value); }
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

        /// <summary>
        /// The total number of files to use as a maximum value
        /// </summary>        
        public int TotalFileCountMaximum
        {
            get { return TotalFileCount == 0 ? 1 : TotalFileCount; }
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

        /// <summary>
        /// The number of BackupActions that have been processed
        /// </summary>
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

        public ICommand BrowseSourceCommand
        { get; private set; }

        public ICommand BrowseDestinationCommand
        { get; private set; }

        public ICommand ExcludeDirectoriesCommand
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
    }
}
