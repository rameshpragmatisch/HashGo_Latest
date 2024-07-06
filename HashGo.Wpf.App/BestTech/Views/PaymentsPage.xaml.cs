using HashGo.Core.Contracts.View;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : BasePage
    {
        public PaymentsPage(PaymentsPageViewModel  paymentsPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();
            this.DataContext = paymentsPageViewModel;
        }
    }
}
