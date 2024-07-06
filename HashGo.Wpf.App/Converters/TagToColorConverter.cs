using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HashGo.Wpf.App.Converters;

public class TagToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null ||
            value == DependencyProperty.UnsetValue)
        {
            return value;
        }

        if (value is string stringValue)
        {
            string colorCode = string.Empty;
            switch(stringValue)
            {
                case "favourite":
                    colorCode = "#F41F67";
                    break;
                case "halal":
                case "flame":
                    colorCode = "#FDCA4B";
                    break;
            }

            return colorCode;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
