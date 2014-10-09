using Catel.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    public class DirectoryModel : ModelBase
    {
        public DirectoryModel(string path)
        {
            var di = new DirectoryInfo(path);
            Name = di.Name;
            FullPath = di.FullName;
            SubDirectories = di.GetDirectories("*", SearchOption.TopDirectoryOnly).Select(d => new DirectoryModel(d.FullName));
        }
        
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            private set { SetValue(NameProperty, value); }
        }

        public static readonly PropertyData FullPathProperty = RegisterProperty("FullPath", typeof(string));

        public string FullPath
        {
            get { return GetValue<string>(FullPathProperty); }
            private set { SetValue(FullPathProperty, value); }
        }

        public static readonly PropertyData IsCheckedProperty = RegisterProperty("IsChecked", typeof(bool), true);

        public bool IsChecked
        {
            get { return GetValue<bool>(IsCheckedProperty); }
           set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly PropertyData SubDirectoriesProperty = RegisterProperty("SubDirectories", typeof(IEnumerable<DirectoryModel>));

        public IEnumerable<DirectoryModel> SubDirectories
        {
            get { return GetValue<IEnumerable<DirectoryModel>>(SubDirectoriesProperty); }
            private set { SetValue(SubDirectoriesProperty, value); }
        }
    }
}
