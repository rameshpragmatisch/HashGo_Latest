using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HashGo.Wpf.App.Converters;

public class StringToColorConverter : IValueConverter
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
            var _color = System.Drawing.ColorTranslator.FromHtml("#"+stringValue);
            return _color;
        }

        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
