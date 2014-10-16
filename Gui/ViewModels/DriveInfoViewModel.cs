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

        public long UsedSpace
        {
            get { return TotalSize - AvailableFreeSpace; }
        }

        public double PercentageFreeSpace
        {
            get 
            {                
                return (100.0 / TotalSize) * AvailableFreeSpace; 
            }
        }

        public string AvailableFreeSpaceString
        {
            get { return NumberFormaters.ConvertBytesToString(_driveInfo.AvailableFreeSpace); }
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
        
        public string SpaceStatusString
        {
            get { return string.Format("{0} free of {1}", AvailableFreeSpaceString, TotalSizeString); }
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

