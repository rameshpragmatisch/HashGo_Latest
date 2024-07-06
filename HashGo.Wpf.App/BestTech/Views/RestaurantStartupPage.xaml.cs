using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for RestaurantStartupPage.xaml
    /// </summary>
    public partial class RestaurantStartupPage : BasePage
    {
        public RestaurantStartupPage(RestaurantStartupPageViewModel restaurantStartupPageViewModel):base()
        {
            InitializeComponent();
            DataContext = restaurantStartupPageViewModel;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            //Storyboard slideInStoryboard = (Storyboard)this.Resources["SlideInStoryboard"];
            //slideInStoryboard.Begin();
            img.Visibility = Visibility.Visible;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            //Storyboard slideOutStoryboard = (Storyboard)this.Resources["SlideOutStoryboard"];
            //slideOutStoryboard.Begin();
            img.Visibility = Visibility.Collapsed;
        }
    }
}
