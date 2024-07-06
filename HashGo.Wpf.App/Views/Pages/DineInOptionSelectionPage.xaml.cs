using HashGo.Domain.ViewModels;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for RestaurantDineInSelectionPage.xaml
    /// </summary>
    public partial class DineInOptionSelectionPage : Page
    {
        public DineInOptionSelectionPage(DineInOptionSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
