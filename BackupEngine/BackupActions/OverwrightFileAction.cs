using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    /// <summary>
    /// A BackupAction to overwrite a file
    /// </summary>
    public class OverwriteFileAction : CopyFileAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="destination">The destination file</param>
        public OverwriteFileAction(string source, string destination)
            : base(source, destination)
        {
        }

        /// <summary>
        /// Executes the BackupAction
        /// </summary>
        /// <returns>If the BackupAction was successful</returns>
        public override bool Execute()
        {
            try
            {
                if (!File.Exists(Source))
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_DOES_NOT_EXIST, Source));
                }
                if (!File.Exists(Destination))
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_DOES_NOT_EXIST, Destination));
                }
                //If its readonly set it back to normal   //Need to "AND" it as it can also be archive, hidden etc 
                if ((File.GetAttributes(Destination) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(Destination, FileAttributes.Normal);
                }
                var fi = new FileInfo(Source);
                fi.CopyTo(Destination, true);
            }
            catch (Exception ex)
            {
                return ActionFailed(ex.Message);
            }
            _status = ActionStatus.Complete;
            return true;
        }

        public override string ActionName
        {
            get { return "Overwrite File"; }
        }
    }
}
