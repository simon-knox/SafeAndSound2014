using System;
using System.IO;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    /// <summary>
    /// A BackupAction to delete a file
    /// </summary>
    public class DeleteFileAction : DirectAction
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The _path to delete the file from</param>
        public DeleteFileAction(string path)
            : base(path)
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
                if (File.Exists(Path))
                {
                    //If its readonly set it back to normal   //Need to "AND" it as it can also be archive, hidden etc 
                    if ((File.GetAttributes(Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        File.SetAttributes(Path, FileAttributes.Normal);
                    }
                    File.Delete(Path);
                }
                else
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_DOES_NOT_EXIST, Path));
                }
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
            get { return "Delete File"; }
        }
    }
}