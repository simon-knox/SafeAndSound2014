using Catel.Data;
using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class DirectoryViewModel2 : ViewModelBase
    {

         public DirectoryViewModel2(string path)
        {
            var di = new DirectoryInfo(path);
            Name = di.Name;
            FullPath = di.FullName;
            SubDirectories = new ObservableCollection<DirectoryViewModel2>(di.GetDirectories("*", SearchOption.TopDirectoryOnly).Select(d => new DirectoryViewModel2(d.FullName)));

            //SubDirectories = new ObservableCollection<DirectoryModel>();
           // SubDirectories.Add(new DirectoryModel(Path.Combine(path, "Books")));
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

        public static readonly PropertyData SubDirectoriesProperty = RegisterProperty("SubDirectories", typeof(ObservableCollection<DirectoryViewModel2>));

        public ObservableCollection<DirectoryViewModel2> SubDirectories
        {
            get { return GetValue<ObservableCollection<DirectoryViewModel2>>(SubDirectoriesProperty); }
            private set { SetValue(SubDirectoriesProperty, value); }
        }
    }
}
