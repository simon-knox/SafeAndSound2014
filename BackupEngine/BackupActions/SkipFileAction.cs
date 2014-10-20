using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    /// <summary>
    /// A BackupAction to skip a file
    /// </summary>
    public class SkipFileAction : DirectAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The file path</param>
        public SkipFileAction(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Executes the BackupAction
        /// </summary>
        /// <returns>If the BackupAction was successful</returns>
        public override bool Execute()
        {
            _status = ActionStatus.Skipped;
            return true;
        }

        public override string ActionName
        {
            get { return "Skip File"; }
        }
    }
}
