using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HashGo.Core.Extensions;


namespace HashGo.Wpf.App.Converters;

public class StringToSentenseCaseStringConverter : IValueConverter
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
            return stringValue.ToSentenseCase();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
