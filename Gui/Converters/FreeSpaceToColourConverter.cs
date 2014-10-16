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
    public class FreeSpaceToColourConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double percentage;
            if(double.TryParse(value.ToString(), out percentage))
            {
                if(percentage < 10)
                    return Application.Current.FindResource("LowSpaceBrush") as SolidColorBrush;              
            }
            return Application.Current.FindResource("SpaceRemainingBrush") as SolidColorBrush;           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}