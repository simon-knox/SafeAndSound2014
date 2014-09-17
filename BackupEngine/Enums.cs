namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    ///<summary>
    /// The type of file change
    ///</summary>
    public enum FileChange
    {
        FolderCreate,
        FolderDelete,
        FileCreate,
        FileOverwrite,
        FileDelete,
        FileSkipped,
        Error
    }

    ///<summary>
    /// The status of the file count
    ///</summary>
    public enum FileCountStatus
    {
        NotCounted,
        Started,
        Finished,
        Error
    }

    /// <summary>
    /// the status of the backup process
    /// </summary>
    public enum BackupProcessingStatus
    {
        NotStarted,
        BuildingActionQueue,
        ActionQueueBuilt,
        ProcessingActionQueue,
        CountingFiles,
        FinishedCountingFiles,
        SkippingFiles,
        FinishedSkippingFiles,
        FinishedProcessingActionQueue,
        Started,
        Finished,
        Error
    }

    /// <summary>
    /// The Status of the BackupAction
    /// </summary>
    public enum ActionStatus
    {
        Pending,
        Complete,
        Skipped,
        Failed
    }

    /// <summary>
    /// The Mode of the BackupSet
    /// </summary>
    public enum BackupSetMode
    {
        Backup,
        Restore
    } ;

    /// <summary>
    /// The frequency of the BackupSchedule
    /// </summary>
    public enum BackupScheduleFrequency
    {
        Manual,
        Hourly,
        Daily,
        Weekly,
        Monthly
    } ;

    public enum StatusCodes
    {
        NoBackupRequired = 101,
        BackupRunSuccessfully = 102,
        ActionQueueBuildStarted = 103,
        ActionQueueBuildFinished = 104,
        ActionQueueProcessingStarted = 105,
        NoBackupSetsPresent = 201,
        BackupSetsChanged = 202,
        BackupError = 900,
        SourceDriveNotAvalaible = 901,
        SourceFolderNotAvaliable = 902,
        DestinationDriveNotAvailable = 903,
        DestinationFolderNotAvaliable = 904,
        DestinationDriveFull = 905,
        ActionQueueBuildFailed = 906
    };


}
