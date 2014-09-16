using Catel;
using Catel.Data;
using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Collections.Generic;
using System.IO;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class ExcludedFilesViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedFilesViewModel"/> class.
        /// </summary>
        public ExcludedFilesViewModel(BackupSet backupSet)
        {
            Argument.IsNotNull(() => backupSet);
            BackupSet = backupSet;

            SourceDirectoryTree = BackupSet.GetSourceDirectoryTree();
            int i = 0;            
        }

        public static readonly PropertyData BackupSetProperty = RegisterProperty("BackupSet", typeof(BackupSet), null);
        [Model]
        public BackupSet BackupSet
        {
            get { return GetValue<BackupSet>(BackupSetProperty); }
            set { SetValue(BackupSetProperty, value); }
        }

        public static readonly PropertyData ExcludedDirectoriesProperty = RegisterProperty("ExcludedDirectories", typeof(HashSet<string>), () => new HashSet<string>());
        ///<summary>
        /// The set of excluded directories from the BackupSet
        ///</summary>        
        [ViewModelToModel("BackupSet")]
        public HashSet<string> ExcludedDirectories
        {
            get { return GetValue<HashSet<string>>(ExcludedDirectoriesProperty); }
            set { SetValue(ExcludedDirectoriesProperty, value); }
        }

        public static readonly PropertyData SourceDirectoryProperty = RegisterProperty("SourceDirectory", typeof(string), string.Empty);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        [ViewModelToModel("BackupSet")]
        public string SourceDirectory
        {
            get { return GetValue<string>(SourceDirectoryProperty); }
            set { SetValue(SourceDirectoryProperty, value); }
        }

        public static readonly PropertyData SourceDirectoryTreeProperty = RegisterProperty("SourceDirectoryTree", typeof(List<DirectoryInfo>), null);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        public List<DirectoryInfo> SourceDirectoryTree
        {
            get { return GetValue<List<DirectoryInfo>>(SourceDirectoryTreeProperty); }
            set { SetValue(SourceDirectoryTreeProperty, value); }
        }

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "View model title"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
    }
}
