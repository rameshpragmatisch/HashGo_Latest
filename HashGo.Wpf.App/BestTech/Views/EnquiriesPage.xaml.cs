using HashGo.Core.Contracts.View;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.ViewModels;
using HashGo.Wpf.App.Services;
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

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for EnquiriesPage.xaml
    /// </summary>
    public partial class EnquiriesPage : BasePage
    {
        public EnquiriesPage(EnquiriesPageViewModel enquiriesPageViewModel,IPopupService popupService) : base(popupService)
        {
            InitializeComponent();
            this.DataContext = enquiriesPageViewModel;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
