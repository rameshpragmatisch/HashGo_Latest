using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
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
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ProcessingPaymentPage.xaml
    /// </summary>
    public partial class ProcessingPaymentPage : Page
    {
        DispatcherTimer timer;
        INavigationService navigationService;
        public ProcessingPaymentPage(INavigationService navigationService)
        {
            InitializeComponent();

            this.navigationService = navigationService;

            this.Loaded += (sender, e) =>
            {
                timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(5),
                };

                timer.Tick += (sender, e) =>
                {
                    navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString());
                };

                timer.Start();
            };

            this.Unloaded += (sender, e) =>
            {
                timer.Stop();
                timer = null;
            };
        }
    }
}
