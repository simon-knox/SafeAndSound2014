using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class DirectoryViewModel3 : ViewModelBase
    {
        private DirectoryInfo _directoryInfo;
        public DirectoryViewModel3(string path, bool isPlaceHolder = false)
        {
            IsPlaceHolder = isPlaceHolder;
            if (!IsPlaceHolder)
            {
                _directoryInfo = new DirectoryInfo(path);
                Name = _directoryInfo.Name;
                FullPath = _directoryInfo.FullName;
                SubDirectories = new ObservableCollection<DirectoryViewModel3>(new[] { new DirectoryViewModel3(Name, true) });
            }
            IsExpanded = false;  

            //SubDirectories = new ObservableCollection<DirectoryModel>();
            // SubDirectories.Add(new DirectoryModel(Path.Combine(path, "Books")));
        }

        public void PopulateSubDirectories()
        {
            try
            {
                SubDirectories = new ObservableCollection<DirectoryViewModel3>(_directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly)
                    .Select(d => new DirectoryViewModel3(d.FullName)));
            }
            catch (UnauthorizedAccessException ex)
            {
                Include = false;
            }
        }
    

        public static readonly PropertyData IsExpandedProperty = RegisterProperty("IsExpanded", typeof(bool));

        public bool IsExpanded
        {
            get { return GetValue<bool>(IsExpandedProperty); }
            set
            {
                SetValue(IsExpandedProperty, value);
                if (value)
                {
                    try
                    {
                        SubDirectories = new ObservableCollection<DirectoryViewModel3>(_directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly)
                            .Select(d => new DirectoryViewModel3(d.FullName)));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Include = false;
                    }
                }
            }
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

        public static readonly PropertyData IncludeProperty = RegisterProperty("Include", typeof(bool), true);

        public bool Include
        {
            get { return GetValue<bool>(IncludeProperty); }
            set { SetValue(IncludeProperty, value); }
        }

        public static readonly PropertyData IsPlaceHolderProperty = RegisterProperty("IsPlaceHolder", typeof(bool), true);
        
        public bool IsPlaceHolder
        {
            get { return GetValue<bool>(IsPlaceHolderProperty); }
            set { SetValue(IsPlaceHolderProperty, value); }
        }

        public static readonly PropertyData SubDirectoriesProperty = RegisterProperty("SubDirectories", typeof(ObservableCollection<DirectoryViewModel3>));

        public ObservableCollection<DirectoryViewModel3> SubDirectories
        {
            get { return GetValue<ObservableCollection<DirectoryViewModel3>>(SubDirectoriesProperty); }
            private set { SetValue(SubDirectoriesProperty, value); }
        }
    }
}

