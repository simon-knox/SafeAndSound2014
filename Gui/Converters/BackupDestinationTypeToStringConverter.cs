using SKnoxConsulting.SafeAndSound.BackupEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SKnoxConsulting.SafeAndSound.Gui.Converters
{
    public class BackupDestinationTypeToStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is BackupDestinationType)
            {
                var backupType = (BackupDestinationType)value;
                switch (backupType)
                {
                    case BackupDestinationType.ExternalDrive:
                        return "External Drive";
                    case BackupDestinationType.NetworkLocation:
                        return "Network Location";
                    default:
                        return "Local Drive";
                }                
            }
            return "Local Drive";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

