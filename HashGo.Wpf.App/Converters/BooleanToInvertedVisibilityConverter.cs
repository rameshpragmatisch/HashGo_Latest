using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters
{

    public class BooleanToInvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null &&
                value == DependencyProperty.UnsetValue)
            {
                return null;
            }

            if (value is bool boolenObject)
            {
                if (!boolenObject)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}