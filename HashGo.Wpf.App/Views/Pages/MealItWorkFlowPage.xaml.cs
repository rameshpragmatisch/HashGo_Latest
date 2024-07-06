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
    /// Interaction logic for MealItWorkFlowPage.xaml
    /// </summary>
    public partial class MealItWorkFlowPage : Page
    {
        public MealItWorkFlowPage(MenuItemDetailViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
