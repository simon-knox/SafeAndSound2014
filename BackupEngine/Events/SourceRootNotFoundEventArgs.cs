using System;

namespace SKnoxConsulting.SafeAndSound.BackupEngine.Events
{
    ///<summary>
    /// The event arguments for source root not found
    ///</summary>
    public class SourceRootNotFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sourceRoot">The source root</param>
        public SourceRootNotFoundEventArgs(string sourceRoot)
        {
            SourceRoot = sourceRoot;
        }

        ///<summary>
        /// The source root
        ///</summary>
        public readonly string SourceRoot;
    }
}
