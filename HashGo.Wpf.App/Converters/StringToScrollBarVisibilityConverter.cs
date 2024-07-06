using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters;

public class StringToScrollBarVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null &&
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        if(value is string stringValue &&
            !string.IsNullOrEmpty(stringValue))
        {
            return ScrollBarVisibility.Auto;
        }

        return ScrollBarVisibility.Disabled;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
