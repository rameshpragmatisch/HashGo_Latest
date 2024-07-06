using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters
{
    public class CollectionToItemTemplateConverter : IValueConverter
    {

        public DataTemplate SingleItemTemplate { get; set; }

        public DataTemplate TwoItemTemplate { get; set; }
        public DataTemplate ThreeItemTemplate { get; set; }

        public DataTemplate FourItemTemplate { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null ||
                value == DependencyProperty.UnsetValue)
            {
                return null;
            }

            if (value is IEnumerable<object> objectCollection && objectCollection.Any())
            {
                var count = objectCollection.Count();

                switch (count)
                {
                    case 1:
                        return SingleItemTemplate;
                        break;
                    case 2:
                        return TwoItemTemplate;
                        break;
                    case 3:
                        return ThreeItemTemplate;
                        break;
                    default:
                        return FourItemTemplate;
                }
            }

            return FourItemTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
