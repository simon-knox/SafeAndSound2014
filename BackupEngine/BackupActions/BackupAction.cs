using Catel.Data;
namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    ///<summary>
    ///</summary>
    public abstract class BackupAction : ModelBase
    {
        private string _errorMessage;

        protected ActionStatus _status;
        protected static string FILE_OR_FOLDER_DOES_NOT_EXIST = "The file or folder {0} does not exist.";
        protected static string FILE_OR_FOLDER_ALREADY_EXISTS = "The file or folder {0} already exists.";


        protected BackupAction()
        {
            _status = ActionStatus.Pending;
        }

        ///<summary>
        /// Executes the BackupAction
        ///</summary>
        ///<returns></returns>
        public abstract bool Execute();

        /// <summary>
        /// The ActionStatus of the BackupAction
        /// </summary>
        public ActionStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        /// <summary>
        /// The error message if an error has occured
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public abstract string ActionName
        {
            get;
        }

        /// <summary>
        /// Marks the BackupAction as failed
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>Returns false</returns>
        protected bool ActionFailed(string message)
        {
            _errorMessage = message;
            _status = ActionStatus.Failed;
            return false;
        }
    }
}
