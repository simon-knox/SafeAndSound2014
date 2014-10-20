using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions
{
    public abstract class DirectAction : BackupAction
    {
        private readonly string _path;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The path to create the folder at</param>
        public DirectAction(string path)
        {
            _path = path;
        }

        /// <summary>
        /// The path to create the folder at
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        public override string ActionName
        {
            get{return string.Empty;}
        }
    }
}
