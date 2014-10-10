using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.Gui.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class DriveInfoViewModel : ViewModelBase
    {
        private DriveInfo _driveInfo;

        public DriveInfoViewModel(DriveInfo driveInfo)
        {
            _driveInfo = driveInfo;
        }

        public long AvailableFreeSpace
        {
            get { return _driveInfo.AvailableFreeSpace; }
        }

        public string DriveFormat
        {
            get { return _driveInfo.DriveFormat; }
        }

        public DriveType DriveType
        {
            get { return _driveInfo.DriveType; }
        }
        public string Name
        {
            get { return _driveInfo.Name; }
        }
        public long TotalFreeSpace
        {
            get { return _driveInfo.TotalFreeSpace; }
        }
        public long TotalSize
        {
            get { return _driveInfo.TotalSize; }
        }

        public string TotalSizeString
        {
            get { return NumberFormaters.ConvertBytesToString(TotalSize); }
        }        

        public string VolumeLabel
        {
            get { return _driveInfo.VolumeLabel; }
        }

        public bool IsReady
        {
            get { return _driveInfo.IsReady; }
        }

        public ImageSource Image
        {
            get { return Win32.GetImageSourceForPath(Name); }
        }

       

        
    }    
}

