using System.Windows.Controls;
using HashGo.Domain.ViewModels;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        public ConfigurationPage(ConfigurationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
