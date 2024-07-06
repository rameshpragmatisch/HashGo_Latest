using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters
{
    public class CollectionToItemPanelConverter : IValueConverter
    {

        public ItemsPanelTemplate WrapPanelItemTemplate { get; set; }

        public ItemsPanelTemplate GridItemTemplate { get; set; }
        public ItemsPanelTemplate StackItemTemplate { get; set; }


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
                        return GridItemTemplate;
                        break;
                    case 2:
                    case 3:
                        return StackItemTemplate;
                        break;
                    default:
                        return WrapPanelItemTemplate;
                }
            }

            return WrapPanelItemTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
