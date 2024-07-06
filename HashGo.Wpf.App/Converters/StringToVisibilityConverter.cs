using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters;

public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null &&
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        if(value is string stringValue &&
            !string.IsNullOrEmpty(stringValue) && stringValue != "No Add-Ons")
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
