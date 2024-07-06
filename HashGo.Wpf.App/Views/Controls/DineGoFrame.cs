using System.Windows;
using System.Windows.Controls;
using HashGo.Wpf.App.Contracts.Views;

namespace HashGo.Wpf.App.Views.Controls
{
    public class HashGoFrame : Frame, IFrame
    {

        public object GetDataContext()
        {
            if (Content is FrameworkElement element)
            {
                return element.DataContext;
            }

            return null;
        }

        public void CleanNavigation()
        {
            while (CanGoBack)
            {
                RemoveBackEntry();
            }
        }
    }
}
