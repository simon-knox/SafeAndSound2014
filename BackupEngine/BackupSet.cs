using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using SKnoxConsulting.SafeAndSound.BackupEngine.Events;
using System.Diagnostics;
using Catel.Data;
using Catel.Runtime.Serialization;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    [Serializable]
    public class BackupSet : SavableModelBase<BackupSet>
    {
        #region public delegates

        /// <summary>
        /// Event handler for when the total file count ends
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        //public delegate void TotalFilesCountedEventHandler(object sender, TotalFilesCountedEventArgs e);

        /// <summary>
        /// Event handler for when a backup error occurs
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        //public delegate void BackupErrorEventHandler(object sender, BackupErrorEventArgs e);

        /// <summary>
        /// Event handler for when a drive is unavailable
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        public delegate void DriveUnavailableEventHandler(object sender, DriveUnavailableEventArgs e);

        /// <summary>
        /// Event handler for when the source root is not found
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        public delegate void SourceRootNotFoundEventHandler(object sender, SourceRootNotFoundEventArgs e);

        /// <summary>
        /// Event handler for when the action queue finishes processing
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        //public delegate void ActionProcessingFromQueueEventHandler(object sender, ActionProcessEventArgs e);

        /// <summary>
        /// Event handler for when an action from the action queue finishes processing
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        //public delegate void ActionProcessedFromQueueEventHandler(object sender, ActionProcessEventArgs e);

        /// <summary>
        /// Event handler for when an action from the action queue fails processing
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        //public delegate void ActionProcessingFailedEventHandler(object sender, ActionProcessEventArgs e);

        #endregion public delegates

        #region private members

        //private readonly Queue<BackupAction> _actionQueue;
        //private readonly Queue<BackupAction> _skipFileActionQueue;
        //private readonly Stack<BackupAction> _deleteStack;

        //private bool _checkFileContents;
        //private string _destinationDirectory;
        //private int _errorCount;

        //private int _fileCopyCount;
        //private int _fileDeleteCount;
        //private int _fileOverwriteCount;
        //private int _fileSkipCount;
        //private int _folderCreateCount;
        //private int _folderDeleteCount;
        //private bool _includeHidden;
        //private bool _includeReadOnly;
        //private bool _includeSystem;
        //private string _name;
        private BackupProcessingStatus _processingStatus = BackupProcessingStatus.NotStarted;
        //private bool _removeDeleted;

        //private string _sourceDirectory;
        //private string _status;
        //private int _totalFileCount;
        //private FileCountStatus _totalFileCountStatus = FileCountStatus.NotCounted;
        private CancellationTokenSource _cancelToken;

        private const string STATUS_READY = "Ready.";
        private const string STATUS_SKIPPING_FILES = "Skipping files ...";
        private const string STATUS_COUNTING_FILES = "Counting files ...";
        private const string STATUS_FILES_COUNTED = "Finished counting files.";
        private const string STATUS_BUILDING_ACTION_QUEUE = "Building Action Queue ...";
        private const string STATUS_ACTION_QUEUE_BUILT = "Finshed building Action Queue.";
        private const string STATUS_CANCELLED = "Cancelled.";
        private const string STATUS_FINISHED = "Finished.";
        #endregion private members

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of The BackupSet</param>
        /// <param name="source">The source directory for the backup</param>
        /// <param name="destination">The destination directory for the backup</param>
        /// <param name="excludedDirectories">A list of directories that should be excluded from the backup</param>
        /// <param name="includeReadOnly">Should read only files be included in the backup</param>
        /// <param name="includeHidden">Should hidden files be included in the backup</param>
        /// <param name="includeSystem">Should system files be included in the backup</param>
        /// <param name="removeDeleted">Should files and folders not is the source be deleted from the destination</param>
        public BackupSet(string name = null, string source = null, string destination = null, HashSet<string> excludedDirectories = null,
                         bool includeReadOnly = true, bool includeHidden = true, bool includeSystem = true,
                         bool removeDeleted = true)
        {
            Name = name;
            SourceDirectory = source;
            DestinationDirectory = destination;
            ExcludedDirectories = excludedDirectories;
            IncludeReadOnly = includeReadOnly;
            IncludeHidden = includeHidden;
            IncludeSystem = includeSystem;
            RemoveDeleted = removeDeleted;
            ActionQueue = new Queue<BackupAction>();
            SkipFileActionQueue = new Queue<BackupAction>();
            DeleteActionStack = new Stack<BackupAction>();
            BackupMode = BackupSetMode.Backup;
            //Schedule = new BackupSchedule(GlobalSettings.ApplicationDataPath + "\\" + Name + ".bus");
            ProcessingStatus = BackupProcessingStatus.NotStarted;
        }

        public BackupSet()
        {
           // int i = 0;
        }

        ///// <summary>
        ///// Deserialization constructor
        ///// </summary>
        ///// <param name="info">The SerializationInfo</param>
        ///// <param name="context">The StreamingContext</param>
        //public BackupSet(SerializationInfo info, StreamingContext context)
        //{
        //    //Get the values from info and assign them to the appropriate properties
        //    Name = (string)info.GetValue("Name", typeof(string));
        //    SourceDirectory = (string)info.GetValue("SourceDirectory", typeof(string));
        //    DestinationDirectory = (string)info.GetValue("DestinationDirectory", typeof(string));
        //    ExcludedDirectories = (HashSet<string>)info.GetValue("ExcludedDirectories", typeof(List<string>));
        //    IncludeReadOnly = (bool)info.GetValue("IncludeReadOnly", typeof(bool));
        //    IncludeHidden = (bool)info.GetValue("IncludeHidden", typeof(bool));
        //    IncludeSystem = (bool)info.GetValue("IncludeSystem", typeof(bool));
        //    RemoveDeleted = (bool)info.GetValue("RemoveDeleted", typeof(bool));
        //    //Schedule = (BackupSchedule)info.GetValue("Schedule", typeof(BackupSchedule)) ??
        //     //          new BackupSchedule(GlobalSettings.ApplicationDataPath + "\\" + Name + ".bus");
        //    _actionQueue = new Queue<BackupAction>();
        //    _skipFileActionQueue = new Queue<BackupAction>();
        //    _deleteStack = new Stack<BackupAction>();
        //    BackupMode = BackupSetMode.Backup;
        //    ProcessingStatus = BackupProcessingStatus.NotStarted;
        //}

        #endregion constructors

        #region public properties

        public static readonly PropertyData BackupModeProperty = RegisterProperty("BackupMode", typeof(BackupSetMode), BackupSetMode.Backup);
        ///<summary>
        /// The Mode that the backup set is is e.g. Backup or Restore
        ///</summary>
        [ExcludeFromSerialization]
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
        public HashSet<string> ExcludedDirectories
        {
            get { return GetValue<HashSet<string>>(ExcludedDirectoriesProperty); }
            set { SetValue(ExcludedDirectoriesProperty, value); }
        }


        public static readonly PropertyData DestinationDirectoryProperty = RegisterProperty("DestinationDirectory", typeof(string), string.Empty);
        ///<summary>
        /// The destination directory of the BackupSet
        ///</summary>
        public string DestinationDirectory
        {
            get { return GetValue<string>(DestinationDirectoryProperty); }
            set { SetValue(DestinationDirectoryProperty, value); }
        }

        public static readonly PropertyData DestinationTypeProperty = RegisterProperty("DestinationType", typeof(BackupDestinationType));
        ///<summary>
        /// The type of destination for the BackupSet
        ///</summary>
        public BackupDestinationType DestinationType
        {
            get { return GetValue<BackupDestinationType>(DestinationTypeProperty); }
            set { SetValue(DestinationTypeProperty, value); }
        }

        public static readonly PropertyData SourceDirectoryProperty = RegisterProperty("SourceDirectory", typeof(string), string.Empty);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        public string SourceDirectory
        {
            get { return GetValue<string>(SourceDirectoryProperty); }
            set { SetValue(SourceDirectoryProperty, value); }
        }

        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty);
        /// <summary>
        /// The name of the BackupSet
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly PropertyData IncludeReadOnlyProperty = RegisterProperty("IncludeReadOnly", typeof(bool), true);
        /// <summary>
        /// Should read only files be included in the backup
        /// </summary>
        public bool IncludeReadOnly
        {
            get { return GetValue<bool>(IncludeReadOnlyProperty); }
            set { SetValue(IncludeReadOnlyProperty, value); }
        }

        public static readonly PropertyData IncludeHiddenProperty = RegisterProperty("IncludeHidden", typeof(bool), true);
        /// <summary>
        /// Should hidden files be included in the backup
        /// </summary>
        public bool IncludeHidden
        {
            get { return GetValue<bool>(IncludeHiddenProperty); }
            set { SetValue(IncludeHiddenProperty, value); }
        }

        public static readonly PropertyData IncludeSystemProperty = RegisterProperty("IncludeSystem", typeof(bool), true);
        /// <summary>
        /// Should system files be included in the backup
        /// </summary>
        public bool IncludeSystem
        {
            get { return GetValue<bool>(IncludeSystemProperty); }
            set { SetValue(IncludeSystemProperty, value); }
        }

        public static readonly PropertyData RemoveDeletedProperty = RegisterProperty("RemoveDeleted", typeof(bool), false);
        /// <summary>
        /// Should files and folders that are in the destination but not the source be deleted
        /// </summary>
        public bool RemoveDeleted
        {
            get { return GetValue<bool>(RemoveDeletedProperty); }
            set { SetValue(RemoveDeletedProperty, value); }
        }

        public static readonly PropertyData ProcessingStatusProperty = RegisterProperty("ProcessingStatus", typeof(BackupProcessingStatus), BackupProcessingStatus.NotStarted);
        /// <summary>
        /// The status of the backup processing
        /// </summary>
        [ExcludeFromSerialization]
        public BackupProcessingStatus ProcessingStatus
        {
            get { return GetValue<BackupProcessingStatus>(ProcessingStatusProperty); }
            set 
            { 
                SetValue(ProcessingStatusProperty, value);
                RaisePropertyChanged(() => Status);
            }
        }

        public static readonly PropertyData StatusProperty = RegisterProperty("Status", typeof(string), STATUS_READY);
        /// <summary>
        /// The status of the BackupSet
        /// </summary>
        [ExcludeFromSerialization]
        public string Status
        {
            get
            {
                switch (ProcessingStatus)
                {
                    case BackupProcessingStatus.NotStarted:
                        return STATUS_READY;
                    case BackupProcessingStatus.BuildingActionQueue:
                        return STATUS_BUILDING_ACTION_QUEUE;
                    case BackupProcessingStatus.ActionQueueBuilt:
                        return STATUS_ACTION_QUEUE_BUILT;
                    case BackupProcessingStatus.CountingFiles:
                        return STATUS_COUNTING_FILES;
                    case BackupProcessingStatus.SkippingFiles:
                        return STATUS_SKIPPING_FILES;
                    case BackupProcessingStatus.Cancelled:
                        return STATUS_CANCELLED;
                    case BackupProcessingStatus.FinishedProcessingActionQueue:
                    case BackupProcessingStatus.Finished:
                        return STATUS_FINISHED;
                    default:
                        return GetValue<string>(StatusProperty);
                }
            }
            private set
            {
                SetValue(StatusProperty, value);
                RaisePropertyChanged(() => ProcessingStatus);
            }
        }

        public static readonly PropertyData TotalFileCountProperty = RegisterProperty("TotalFileCount", typeof(int), 0);
        /// <summary>
        /// The total number of files in the BackupSet
        /// </summary>
        [ExcludeFromSerialization]
        public int TotalFileCount
        {
            get { return GetValue<int>(TotalFileCountProperty); }
            private set
            {
                SetValue(TotalFileCountProperty, value);
                RaisePropertyChanged(() => TotalFileCountMaximum);
            }
        }

        /// <summary>
        /// The total number of files to use as a maximum value
        /// </summary>
        [ExcludeFromSerialization]
        public int TotalFileCountMaximum
        {
            get { return TotalFileCount == 0 ? 1 : TotalFileCount; }
        }

        public static readonly PropertyData FolderDeleteCountProperty = RegisterProperty("FolderDeleteCount", typeof(int), 0);
        /// <summary>
        /// The number of folders that have been deleted
        /// </summary>
        [ExcludeFromSerialization]
        public int FolderDeleteCount
        {
            get { return GetValue<int>(FolderDeleteCountProperty); }
            private set { SetValue(FolderDeleteCountProperty, value); }
        }

        public static readonly PropertyData FileDeleteCountProperty = RegisterProperty("FileDeleteCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been deleted
        ///</summary>
        [ExcludeFromSerialization]
        public int FileDeleteCount
        {
            get { return GetValue<int>(FileDeleteCountProperty); }
            private set { SetValue(FileDeleteCountProperty, value); }
        }

        public static readonly PropertyData FileCopyCountProperty = RegisterProperty("FileCopyCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been copied
        ///</summary>
        [ExcludeFromSerialization]
        public int FileCopyCount
        {
            get { return GetValue<int>(FileCopyCountProperty); }
            private set
            {
                SetValue(FileCopyCountProperty, value);
                RaisePropertyChanged(() => ProcessingProgressCount);
            }
        }

        public static readonly PropertyData FolderCreateCountProperty = RegisterProperty("FolderCreateCount", typeof(int), 0);
        ///<summary>
        /// The number of folders that have been created
        ///</summary>
        [ExcludeFromSerialization]
        public int FolderCreateCount
        {
            get { return GetValue<int>(FolderCreateCountProperty); }
            private set { SetValue(FolderCreateCountProperty, value); }
        }

        public static readonly PropertyData FileOverwriteCountProperty = RegisterProperty("FileOverwriteCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been overwritten
        ///</summary>
        [ExcludeFromSerialization]
        public int FileOverwriteCount
        {
            get { return GetValue<int>(FileOverwriteCountProperty); }
            private set 
            {
                SetValue(FileOverwriteCountProperty, value);
                RaisePropertyChanged(() => ProcessingProgressCount);
            }
        }

        public static readonly PropertyData FileSkipCountProperty = RegisterProperty("FileSkipCount", typeof(int), 0);
        ///<summary>
        /// The number of files that have been skipped
        ///</summary>
        [ExcludeFromSerialization]
        public int FileSkipCount
        {
            get { return GetValue<int>(FileSkipCountProperty); }
            private set 
            { 
                SetValue(FileSkipCountProperty, value);
                RaisePropertyChanged(() => ProcessingProgressCount);            
            }
        }

        public static readonly PropertyData ErrorCountProperty = RegisterProperty("ErrorCount", typeof(int), 0);
        /// <summary>
        /// The number of errors that have occured
        /// </summary>
        [ExcludeFromSerialization]
        public int ErrorCount
        {
            get { return GetValue<int>(ErrorCountProperty); }
            private set
            {
                SetValue(ErrorCountProperty, value);
                RaisePropertyChanged(() => ProcessingProgressCount);
            }
        }

        public static readonly PropertyData ProcessingProgressCountProperty = RegisterProperty("ProcessingProgressCount", typeof(int), 0);      
        /// <summary>
        /// The number of BackupActions that have been processed
        /// </summary>
        [ExcludeFromSerialization]
        public int ProcessingProgressCount
        {
            get { return FileCopyCount + FileOverwriteCount + FileSkipCount + ErrorCount; }
        }

        public static readonly PropertyData ActionQueueProperty = RegisterProperty("ActionQueue", typeof(Queue<BackupAction>), new Queue<BackupAction>());
        /// <summary>
        /// The queue of BackupActions
        /// </summary>
        [ExcludeFromSerialization]
        public Queue<BackupAction> ActionQueue
        {
            get { return GetValue<Queue<BackupAction>>(ActionQueueProperty); }
            private set { SetValue(ActionQueueProperty, value); }
        }

        public static readonly PropertyData SkipFileActionQueueProperty = RegisterProperty("SkipFileActionQueue", typeof(Queue<BackupAction>), new Queue<BackupAction>());
        /// <summary>
        /// The queue of BackupActions that will be skipped
        /// </summary>
        [ExcludeFromSerialization]
        public Queue<BackupAction> SkipFileActionQueue
        {
            get { return GetValue<Queue<BackupAction>>(SkipFileActionQueueProperty); }
            private set { SetValue(SkipFileActionQueueProperty, value); }
        }

        public static readonly PropertyData DeleteActionStackProperty = RegisterProperty("DeleteActionStack", typeof(Stack<BackupAction>), new Stack<BackupAction>());
        /// <summary>
        /// The stack of Delete BackupActions
        /// </summary>
        [ExcludeFromSerialization]
        public Stack<BackupAction> DeleteActionStack
        {
            get { return GetValue<Stack<BackupAction>>(DeleteActionStackProperty); }
            private set { SetValue(DeleteActionStackProperty, value); }
        }

        ///// <summary>
        ///// The BackupSchedule for this BackupSet
        ///// </summary>
        //public BackupSchedule Schedule { get; set; }

        #endregion public properties

        #region public events

        /// <summary>
        /// Occurs when the total file count starts
        /// </summary>
        //public event TotalFileCountStartedEventHandler TotalFileCountStarted;

        /// <summary>
        /// Occurs when the total files are counted
        /// </summary>
        // public event TotalFilesCountedEventHandler TotalFilesCounted;

        /// <summary>
        /// Occurs when a drive is unavailable
        /// </summary>
        public event DriveUnavailableEventHandler DriveUnavailable;

        /// <summary>
        /// Occurs when the source root is not found
        /// </summary>
        public event SourceRootNotFoundEventHandler SourceRootNotFound;

        /// <summary>
        /// Occurs when the action queue starts building
        /// </summary>
        //public event BuildActionQueueStartedEventHandler BuildActionQueueStarted;

        /// <summary>
        /// Occurs when an action is added to the action queue
        /// </summary>
        public event EventHandler ActionAddedToQueue;

        /// <summary>
        /// Occurs when the action queue finishes building
        /// </summary>
        //public event BuildActionQueueFinishedEventHandler BuildActionQueueFinished;

        /// <summary>
        /// Occurs when the action queue finishes building
        /// </summary>
        //public event BuildActionQueueFailedEventHandler BuildActionQueueFailed;

        /// <summary>
        /// Occurs when the action queue starts processing
        /// </summary>
        //public event ProcessActionQueueStartedEventHandler ProcessActionQueueStarted;

        /// <summary>
        /// Occurs when an action from the action queue begins processing
        /// </summary>
        //public event ActionProcessingFromQueueEventHandler ActionProcessingFromQueue;

        /// <summary>
        /// Occurs when an action from the action queue ends processing
        /// </summary>
        //public event ActionProcessedFromQueueEventHandler ActionProcessedFromQueue;

        /// <summary>
        /// Occurs when an action from the action queue fails processing
        /// </summary>
        //public event ActionProcessingFailedEventHandler ActionProcessingFailed;

        /// <summary>
        /// Occurs when the action queu finishes processing
        /// </summary>
        //public event ProcessActionQueueFinishedEventHandler ProcessActionQueueFinished;

        #endregion public events

        #region public methods

        ///// <summary>
        ///// Serialization function
        ///// </summary>
        ///// <param name="info">The SerializationInfo</param>
        ///// <param name="context">The SerializationContext</param>
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("Name", Name);
        //    info.AddValue("SourceDirectory", SourceDirectory);
        //    info.AddValue("DestinationDirectory", DestinationDirectory);
        //    info.AddValue("ExcludedDirectories", ExcludedDirectories);
        //    info.AddValue("IncludeReadOnly", IncludeReadOnly);
        //    info.AddValue("IncludeHidden", IncludeHidden);
        //    info.AddValue("IncludeSystem", IncludeSystem);
        //    info.AddValue("RemoveDeleted", RemoveDeleted);
        //    //info.AddValue("Schedule", Schedule);
        //}

        /// <summary>
        /// Returns the BackupSet Name
        /// </summary>
        /// <returns>A string containing the BackupSet Name</returns>
        public override string ToString()
        {
            return Name;
        }

        ///<summary>
        /// Saves the BackupSet
        ///</summary>
        //public void Save()
        //{
        //    using (Stream stream = File.Open(string.Format("{0}\\{1}.bus", GlobalSettings.ApplicationDataPath, Name),
        //                              FileMode.Create))
        //    {
        //        var bformatter = new BinaryFormatter();
        //        bformatter.Serialize(stream, this);
        //    }
        //}

        public DirectoryModel GetSourceDirectoryTree()
        {    
            //return IOHelper.GetDirectories(SourceDirectory);
            return new DirectoryModel(SourceDirectory);
        }

        /// <summary>
        /// Runs the backup
        /// </summary>
        public void RunBackup()
        {
            ClearProgressCounts();
            _cancelToken = new CancellationTokenSource();
            
            Task.Factory.StartNew(() =>
            {
                if (CheckDriveExists(DestinationDirectory.Substring(0, 1)))
                {
                    BuildActionQueue();
                    int nonFileActions = ActionQueue
                        .Where(aq => aq.GetType() == typeof(CreateFolderAction))
                        .Count();
                    TotalFileCount = SkipFileActionQueue.Count + ActionQueue.Count + DeleteActionStack.Count - nonFileActions;
                }
                else
                {
                    _cancelToken.Cancel();
                }
            })
                .ContinueWith((t) =>
                {
                    ApplyActions();
                })
                .ContinueWith((t) =>
                {
                    ProcessingStatus = _cancelToken.IsCancellationRequested ? BackupProcessingStatus.Cancelled :
                        BackupProcessingStatus.Finished;
                });
        }

        public void CancelBackup()
        {
            _cancelToken.Cancel();
        }

        #endregion public methods

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError(NameProperty, "The backup set name is required"));
            }
            if (string.IsNullOrWhiteSpace(SourceDirectory))
            {
                validationResults.Add(FieldValidationResult.CreateError(SourceDirectoryProperty, "The source directory is required"));
            }

            if (string.IsNullOrWhiteSpace(DestinationDirectory))
            {
                validationResults.Add(FieldValidationResult.CreateError(DestinationDirectoryProperty, "The destination directory is required"));
            }
        }

        #region private methods

        private void BuildActionQueue()
        {
            ProcessingStatus = BackupProcessingStatus.BuildingActionQueue;
            //Clear the action Queues
            ActionQueue.Clear();
            SkipFileActionQueue.Clear();
            DeleteActionStack.Clear();

            if (_cancelToken.IsCancellationRequested)
            {
                ProcessingStatus = BackupProcessingStatus.Cancelled;
                return;
            }

            if (CheckDriveExists(SourceDirectory.Substring(0, 1)) &&
                CheckSourceRootExists() &&
                CheckDriveExists(DestinationDirectory.Substring(0, 1)))
            {
                if (BackupMode == BackupSetMode.Backup && RemoveDeleted)
                {
                    List<DirectoryInfo> destDirs = IOHelper.GetDirectoriesPostOrder(DestinationDirectory);
                    if (destDirs != null)
                    {
                        foreach (DirectoryInfo di in destDirs)
                        {                          
                            if (_cancelToken.IsCancellationRequested)
                            {
                                ProcessingStatus = BackupProcessingStatus.Cancelled;
                                return;
                            }
                            ProcessDestinationDirectoryForPossibleDelete(di);
                        }
                    }
                }

                var dirs = IOHelper.GetDirectories(SourceDirectory);
                if (dirs != null)
                {
                    foreach (DirectoryInfo di in dirs)
                    {
                        //Check if the directory should be excluded
                        if (ExcludedDirectories == null || !ExcludedDirectories.Contains(di.FullName))
                        {
                            if (_cancelToken.IsCancellationRequested)
                            {
                                ProcessingStatus = BackupProcessingStatus.Cancelled;
                                return;
                            }
                            ProcessDirectoryForActions(di);
                        }
                        else
                        {
                            IOHelper.GetFiles(di.FullName)
                                .ForEach(fi => SkipFileActionQueue.Enqueue(new SkipFileAction(fi.FullName)));
                        }
                    }
                }
            }
            ProcessingStatus = BackupProcessingStatus.ActionQueueBuilt;
        }

        private void ProcessDirectoryForActions(DirectoryInfo di)
        {
            //Get the destination directory
            string newFolder = di.FullName.Substring(SourceDirectory.Length);
            if (newFolder.StartsWith(@"\"))
            {
                newFolder = newFolder.Substring(1);
            }
            string destDirectory = Path.Combine(DestinationDirectory, newFolder);
            var destDir = new DirectoryInfo(destDirectory);

            //Create the destination directory if it doesn't already exist
            //if (!Directory.Exists(destDirectory))
            if (!destDir.Exists)
            {
                ActionQueue.Enqueue(new CreateFolderAction(destDir.FullName));
                OnActionAddedToQueue(new EventArgs());
            }

            //Now check each file in the source directory for the following conditions
            //  * In the source directory but not in the destination directory
            //  * file in source and destination directory are different
            try
            {
                foreach (FileInfo fi in di.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                {
                    //FileBeginProcessingEventArgs ea = new FileBeginProcessingEventArgs(fi.FullName);
                    //OnFileBeginProcessing(ea);
                    string destPath = Path.Combine(destDir.FullName, fi.Name);

                    //Check file attributes
                    if ((!IncludeHidden && fi.Attributes.ToString().Contains("Hidden")) ||
                        (!IncludeReadOnly && fi.Attributes.ToString().Contains("ReadOnly")) ||
                        (!IncludeSystem && fi.Attributes.ToString().Contains("System")))
                    {
                        SkipFileActionQueue.Enqueue(new SkipFileAction(fi.FullName));
                        OnActionAddedToQueue(new EventArgs());
                    }
                    else
                    {
                        //Check if the file exists
                        ///TODO: Probably only need copy action
                        if (!File.Exists(destPath))
                        {
                            ActionQueue.Enqueue(new CopyFileAction(fi.FullName, destPath));
                            OnActionAddedToQueue(new EventArgs());
                        }
                        else //Only copy if it has changed
                        {
                            var fiD = new FileInfo(destPath);
                            if (fi.LastWriteTimeUtc < fiD.LastWriteTimeUtc.AddSeconds(-5) ||
                                fi.Length != fiD.Length)
                            {
                                ActionQueue.Enqueue(new OverwriteFileAction(fi.FullName, destPath));
                                OnActionAddedToQueue(new EventArgs());
                            }
                            else
                            {
                                SkipFileActionQueue.Enqueue(new SkipFileAction(fi.FullName));
                                OnActionAddedToQueue(new EventArgs());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}: {1} {2}",
                            "ProcessDirectoryForActions", di.FullName, ex.Message));
            }
        }

        private bool CheckDriveExists(string driveLetter)
        {
            if (driveLetter != "\\")
            {
                var di = new DriveInfo(driveLetter);
                if (!di.IsReady)
                {
                    var e = new DriveUnavailableEventArgs(driveLetter, di.DriveType);
                    OnDriveUnavailable(e);
                    return false;
                }
            }
            return true;
        }

        private void OnDriveUnavailable(DriveUnavailableEventArgs e)
        {
            if (DriveUnavailable != null)
            {
                DriveUnavailable(this, e);
            }
        }

        private bool CheckSourceRootExists()
        {
            if (Directory.Exists(SourceDirectory))
            {
                return true;
            }
            OnSourceRootNotFound(new SourceRootNotFoundEventArgs(SourceDirectory));
            return false;

        }

        private void OnSourceRootNotFound(SourceRootNotFoundEventArgs e)
        {
            if (SourceRootNotFound != null)
            {
                SourceRootNotFound(this, e);
            }
        }

        private void ProcessDestinationDirectoryForPossibleDelete(DirectoryInfo di)
        {
            //Get the source directory
            string srcDirectory = SourceDirectory + di.FullName.Substring(DestinationDirectory.Length);
            var srcDir = new DirectoryInfo(srcDirectory);
            try
            {
                //Delete the destination directory if it doesn't exist at the source
                if (!Directory.Exists(srcDirectory))
                {
                    try
                    {
                        ActionQueue.Enqueue(new DeleteFolderAction(di.FullName));
                        OnActionAddedToQueue(new EventArgs());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(string.Format("{0}: {1} {2}",
                            "ProcessDestinationDirectoryForPossibleDelete", di.FullName, ex.Message));
                    }
                }
                else
                {
                    foreach (FileInfo fi in di.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                    {
                        string srcPath = srcDir + "\\" + fi.Name;
                        try
                        {
                            Debug.WriteLine(di.FullName);

                            //Delete the destination file if it doesn't exist at the source
                            if (!File.Exists(srcPath))
                            {
                                ActionQueue.Enqueue(new DeleteFileAction(fi.FullName));
                                OnActionAddedToQueue(new EventArgs());
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(string.Format("{0}: {1} {2}",
                            "ProcessDestinationDirectoryForPossibleDelete", di.FullName, ex.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}: {1} {2}",
                            "ProcessDestinationDirectoryForPossibleDelete", di.FullName, ex.Message));
            }
        }

        private void ApplyActions()
        {
            if (SkipFileActionQueue.Count > 0)
            {
                ProcessingStatus = BackupProcessingStatus.SkippingFiles;
                foreach (SkipFileAction sfa in SkipFileActionQueue)
                {
                    sfa.Status = ActionStatus.Skipped;
                }
                FileSkipCount = SkipFileActionQueue.Count;
            }

           // Status = "TEST!!!";
           ProcessingStatus = BackupProcessingStatus.FinishedCountingFiles;

            foreach (BackupAction act in ActionQueue)
            {
                if (_cancelToken.IsCancellationRequested)
                {
                    return;
                }
                Status = "Processing " + GetActionPath(act);
                if (act.Execute())
                {
                    Type actionType = act.GetType();
                    if (actionType == typeof(CopyFileAction))
                    {
                        FileCopyCount++;
                    }
                    else if (actionType == typeof(OverwriteFileAction))
                    {
                        FileOverwriteCount++;
                    }
                    else if (actionType == typeof(DeleteFileAction))
                    {
                        FileDeleteCount++;
                    }
                    else if (actionType == typeof(CreateFolderAction))
                    {
                        FolderCreateCount++;
                    }
                    else if (actionType == typeof(DeleteFolderAction))
                    {
                        FolderDeleteCount++;
                    }
                }
                else
                {
                    ErrorCount++;
                }
            }
        }

        private void ClearProgressCounts()
        {
            FolderDeleteCount = 0;
            FolderCreateCount = 0;
            FileCopyCount = 0;
            FileOverwriteCount = 0;
            FileSkipCount = 0;
            ErrorCount = 0;
        }


        private void OnActionAddedToQueue(EventArgs e)
        {
            if (ActionAddedToQueue != null)
            {
                ActionAddedToQueue(this, e);
            }
        }

        private string GetActionPath(BackupAction action)
        {
            if (action is CopyFileAction)
                return ((CopyFileAction)action).Source;
            if (action is DirectAction)
                return ((DirectAction)action).Path;
            return string.Empty;
        }

        #endregion private methods

    }
}
