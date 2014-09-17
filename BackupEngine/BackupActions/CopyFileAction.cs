using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    ///<summary>
    /// A BackupAction to create a file
    ///</summary>
    public class CopyFileAction : BackupAction
    {
        private readonly string _destination;
        private readonly string _source;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="source">The source directory</param>
        ///<param name="destination">The destination directory</param>
        public CopyFileAction(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        /// <summary>
        /// The source directory
        /// </summary>
        public string Source
        {
            get { return _source; }
        }

        /// <summary>
        /// The destination directory
        /// </summary>
        public string Destination
        {
            get { return _destination; }
        }

        /// <summary>
        /// Executes the BackupAction
        /// </summary>
        /// <returns>If the BackupAction was successful</returns>
        public override bool Execute()
        {
            try
            {
                if (!File.Exists(_source))
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_DOES_NOT_EXIST, _source));
                }
                if (File.Exists(_destination))
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_ALREADY_EXISTS, _destination));
                }
                var fi = new FileInfo(_source);
                fi.CopyTo(_destination, false);
            }
            catch (Exception ex)
            {
                return ActionFailed(ex.Message);
            }
            _status = ActionStatus.Complete;
            return true;
        }
    }
}
