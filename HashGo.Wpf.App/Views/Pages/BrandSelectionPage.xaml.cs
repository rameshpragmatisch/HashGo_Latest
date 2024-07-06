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
    /// Interaction logic for BrandSelectionPage.xaml
    /// </summary>
    public partial class BrandSelectionPage : Page
    {
        private BrandSelectionViewModel brandSelectionViewModel;
        public BrandSelectionPage(BrandSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = brandSelectionViewModel= viewModel;
            this.Loaded += BrandSelectionPage_Loaded;
        }

        private void BrandSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            brandSelectionViewModel.InitializeDataCommand.Execute(this);
        }

    }
}
