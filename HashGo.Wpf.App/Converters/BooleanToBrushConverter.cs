using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HashGo.Wpf.App.Converters;

public class BooleanToBrushConverter : IValueConverter
{

    public Brush Brush { get; set; }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null ||
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        if (value is bool boolValue && this.Brush != null)
        {
            return Brush;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
