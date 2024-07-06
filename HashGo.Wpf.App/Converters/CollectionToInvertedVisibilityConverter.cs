using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters;

public class CollectionToInvertedVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null &&
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        if(value is IEnumerable<object> objectCollection)
        {
            if(objectCollection.Any()) 
            {
                return Visibility.Hidden;
            }
            else 
            {
                return Visibility.Visible;
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
