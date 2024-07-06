using HashGo.Wpf.App.BestTech.ViewModels.Popups;
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
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Popups
{
    /// <summary>
    /// Interaction logic for ConfirmCustomerDetailsPopup.xaml
    /// </summary>
    public partial class ConfirmCustomerDetailsPopup : Window
    {
        public ConfirmCustomerDetailsPopup(ConfirmCustomerDetailsPopupViewModel confirmCustomerDetailsPopupViewModel)
        {
            InitializeComponent();

            this.DataContext = confirmCustomerDetailsPopupViewModel;
        }
    }
}
