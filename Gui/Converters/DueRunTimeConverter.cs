using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SKnoxConsulting.SafeAndSound.Gui.Converters
{
    public class DueRunTimeConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dateTime;
            if (DateTime.TryParse(value.ToString(), out dateTime))
            {
                if (dateTime == DateTime.MaxValue)
                    return string.Empty;
                if (dateTime > DateTime.MinValue)
                    return dateTime.ToString("dd MMM yyyy HH:mm ");
            }
            return "Immediately";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}