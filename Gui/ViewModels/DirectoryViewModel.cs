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
    public class DirectoryViewModel : ViewModelBase
    {
        private bool _include;

        public DirectoryViewModel(string path, bool include = true)
        {
            _include = include;

            var di = new DirectoryInfo(path);
            Name = di.Name;
            FullName = di.FullName;

            SubDirectories = new ObservableCollection<DirectoryViewModel>(
                di.GetDirectories("*", SearchOption.TopDirectoryOnly)
                .Select(d=>new DirectoryViewModel(d.FullName)));
        }

        public string Name
        { get; set; }

        public string FullName
        { get; set; }

        public ObservableCollection<DirectoryViewModel> SubDirectories
        {
            get;
            set;
        }

        public bool Include
        {
            get { return _include; }
            set
            {
                _include = value;
                RaisePropertyChanged(() => Include);
                SetSubDirectoriesInclude(_include);
            }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }

        }

        public void SetSubDirectoriesInclude(bool include)
        {
            foreach(var sd in SubDirectories)
            {
                sd.Include = include;
            }
        }       

        public IEnumerable<DirectoryInfo>  GetExcludedDirectories()
        {
            return SubDirectories.Where(d=>!d.Include)
                                       .Select(d => new DirectoryInfo(d.FullName));
        }
     
        public IEnumerable<DirectoryInfo> ExcludedDirectories
        { 
            get { return SubDirectories.Where(d=>!d.Include)
                                       .Select(d => new DirectoryInfo(d.FullName));}
        }
    }    
}
