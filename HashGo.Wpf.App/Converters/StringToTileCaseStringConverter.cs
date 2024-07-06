using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HashGo.Core.Extensions;


namespace HashGo.Wpf.App.Converters;

public class StringToTileCaseStringConverter : IValueConverter
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
            return stringValue.ToTitleCase();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
