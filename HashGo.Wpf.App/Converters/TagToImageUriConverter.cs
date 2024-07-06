using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HashGo.Wpf.App.Converters;

public class TagToImageUriConverter : IValueConverter
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
            string imageUri = string.Empty;
            switch(stringValue)
            {
                case "favourite":
                    imageUri = "/Resources/Images/heart.png";
                    break;
                case "halal":
                    imageUri = "/Resources/Images/halal.png";
                    break;
                case "flame":
                    imageUri = "/Resources/Images/fire.png";
                    break;
            }

            return imageUri;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
