using HashGo.Domain.ViewModels;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for BrandStartPage.xaml
    /// </summary>
    public partial class BrandStartPage : Page
    {
        private BrandStartViewModel brandStartViewModel;
        public BrandStartPage(BrandStartViewModel viewModel)
        {
            InitializeComponent();
            DataContext = brandStartViewModel = viewModel;
            this.Loaded += BrandStartPage_Loaded;
        }

        private void BrandStartPage_Loaded(object sender, RoutedEventArgs e)
        {
            brandStartViewModel.InitializeDataCommand.Execute(this);
        }
    }
}
