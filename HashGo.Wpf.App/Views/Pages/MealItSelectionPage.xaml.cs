using HashGo.Domain.ViewModels;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for MealItSelectionPage.xaml
    /// </summary>
    public partial class MealItSelectionPage : Page
    {
        public MealItSelectionPage(MealItSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
