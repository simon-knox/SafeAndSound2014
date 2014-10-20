using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    /// <summary>
    /// A BackupAction to delete a folder
    /// </summary>
    public class DeleteFolderAction : DirectAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The path to delete the folder from</param>
        public DeleteFolderAction(string path)
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
                if (Directory.Exists(Path))
                {
                    //Check for read only files as they wil prevent the folder being deleted
                    DirectoryInfo di = new DirectoryInfo(Path);
                    foreach (FileInfo f in di.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                    {
                        //If its readonly set it back to normal   //Need to "AND" it as it can also be archive, hidden etc 
                        if ((File.GetAttributes(f.FullName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(f.FullName, FileAttributes.Normal);
                        }
                    }
                    Directory.Delete(Path, true);
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
            get { return "Delete Folder"; }
        }
    }
}