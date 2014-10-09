using Catel;
using Catel.Data;
using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            SourceDirectoryTree = new DirectoryViewModel2(BackupSet.SourceDirectory);

            TestDirSource = new DirectoryViewModel2(backupSet.SourceDirectory);

            Items = new ObservableCollection<DirectoryViewModel>();
            Items.Add(new DirectoryViewModel(backupSet.SourceDirectory));

            int i = 0;            
        }

        public ObservableCollection<DirectoryViewModel> Items
        { get; set; }

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

        public static readonly PropertyData SourceDirectoryTreeProperty = RegisterProperty("SourceDirectoryTree", typeof(DirectoryViewModel2), null);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        public DirectoryViewModel2 SourceDirectoryTree
        {
            get { return GetValue<DirectoryViewModel2>(SourceDirectoryTreeProperty); }
            set { SetValue(SourceDirectoryTreeProperty, value); }
        }

        public static readonly PropertyData TestDirSourceProperty = RegisterProperty("TestDirSource", typeof(DirectoryViewModel2), null);
        /// <summary>
        /// The source directory of the BackupSet
        /// </summary>
        public DirectoryViewModel2 TestDirSource
        {
            get { return GetValue<DirectoryViewModel2>(TestDirSourceProperty); }
            set { SetValue(TestDirSourceProperty, value); }
        }



        //public static readonly PropertyData SourceDirectoryModelProperty = RegisterProperty("SourceDirectoryModel", typeof(DirectoryModel), null);
        ///// <summary>
        ///// The source directory of the BackupSet
        ///// </summary>
        //public DirectoryModel SourceDirectoryModel
        //{
        //    get { return GetValue<DirectoryModel>(SourceDirectoryModelProperty); }
        //    set { SetValue(SourceDirectoryModelProperty, value); }
        //}

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
