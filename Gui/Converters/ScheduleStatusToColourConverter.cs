using SKnoxConsulting.SafeAndSound.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace SKnoxConsulting.SafeAndSound.Gui.Converters
{
    public class ScheduleStatusToColourConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is BackupSetScheduleStatus)
            {
            BackupSetScheduleStatus status = (BackupSetScheduleStatus)value;
                switch(status)
                {
                    case BackupSetScheduleStatus.UpToDate:
                        return Application.Current.FindResource("OKBrush") as SolidColorBrush;
                    case BackupSetScheduleStatus.LastRunHadErrors:
                        return Application.Current.FindResource("ErrorBrush") as SolidColorBrush;
                    default:
                        return Application.Current.FindResource("WarningBrush") as SolidColorBrush;
                }
            }
            return null;                   
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
