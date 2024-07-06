using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters;

public class StringToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null &&
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        var returnValue = (value is string stringValue &&
            !string.IsNullOrEmpty(stringValue));

        return returnValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
