using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HashGo.Wpf.App.Converters;

public class WaitingTimeToBrushConverter : IValueConverter
{

    public Brush SmallWaitTimeBrush { get; set; } = new SolidColorBrush(Colors.Green);
    public Brush MediumWaitTimeBrush { get; set; } = new SolidColorBrush(Colors.Orange);

    public Brush MediumLargeWaitTimeBrush { get; set; } = new SolidColorBrush(Colors.Red);

    public Brush LargeWaitTimeBrush { get; set; } = new SolidColorBrush(Colors.DarkRed);



    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null ||
            value == DependencyProperty.UnsetValue)
        {
            return null;
        }

        if (value is string waitingTime)
        {
            // Parse the waiting time string to extract numerical values
            var timeParts = waitingTime.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (timeParts.Length >= 2 &&
                int.TryParse(timeParts[0], out int minTime) &&
                int.TryParse(timeParts[1], out int maxTime))
            {
                // Use the minTime and maxTime to determine the brush color
                if (minTime >= 10 && maxTime <= 20)
                {
                    return SmallWaitTimeBrush;
                }
                else if (minTime >= 20 && maxTime <= 30)
                {
                    return MediumWaitTimeBrush;
                }
                else if (minTime >= 30 && maxTime <= 40)
                {
                    return MediumLargeWaitTimeBrush;
                }
                else if (minTime > 40)
                {
                    return LargeWaitTimeBrush;
                }
                else
                {
                    return LargeWaitTimeBrush;
                }
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
