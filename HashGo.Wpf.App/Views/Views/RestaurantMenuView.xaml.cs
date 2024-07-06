using HashGo.Core.Contracts.View;
using HashGo.Domain.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HashGo.Wpf.App.Views.Views
{
    /// <summary>
    /// Interaction logic for RestaurantMenuView.xaml
    /// </summary>
    public partial class RestaurantMenuView : UserControl, IView, IHasDataContext
    {
        private BrandMenuViewModel BrandMenuViewModel;
        public RestaurantMenuView(BrandMenuViewModel viewModel)
        {
            InitializeComponent();
            DataContext = BrandMenuViewModel= viewModel;
            this.lstMenuGroup.Loaded += this.lstMenuGroup_Loaded;
            this.lstMenuGroup.SelectionChanged += this.lstMenuGroup_SelectionChanged;
        }

        private void lstMenuGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lstMenu is UIElement uiElement)
            {
                var scrollViewer = GetScrollViewer(uiElement);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToTop();
                }
            }
        }

        public static ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }

        private void lstMenuGroup_Loaded(object sender, RoutedEventArgs e)
        {
            lstMenuGroup.ScrollIntoView(BrandMenuViewModel.SelectedCategory);
        }
    }
}
    