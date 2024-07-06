using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters
{
    public class ScreenOrientationVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            string param = System.Convert.ToString(parameter);

            if (screenWidth > screenHeight)
            {
                if (param == "LANDSCAPE")
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
            else
            {
                if (param == "PORTRAIT")
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
