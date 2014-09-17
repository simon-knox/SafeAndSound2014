using System;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    ///<summary>
    /// A Backup error
    ///</summary>
    public class BackupError
    {
        private readonly string _destination;
        private readonly Exception _ex;
        private readonly string _source;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="source">The source location</param>
        ///<param name="destination">The destination location</param>
        ///<param name="exception">The exception</param>
        public BackupError(string source, string destination, Exception exception)
        {
            Source = source;
            Destination = destination;
            Ex = exception;
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage
        {
            get { return Ex.Message; }
        }

        /// <summary>
        /// The source location
        /// </summary>
        public string Source
        {
            get;
            private set;
        }

        /// <summary>
        /// The destination location
        /// </summary>
        public string Destination
        {
            get;
            private set;
        }

        /// <summary>
        /// The exception
        /// </summary>
        public Exception Ex
        {
            get;
            private set;
        }
    }
}
