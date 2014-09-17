using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    ///<summary>
    /// A BackupAction for creating a folder
    ///</summary>
    public class CreateFolderAction : DirectAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The path to create the folder at</param>
        public CreateFolderAction(string path)
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
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                else
                {
                    return ActionFailed(string.Format(FILE_OR_FOLDER_ALREADY_EXISTS, Path));
                }
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
