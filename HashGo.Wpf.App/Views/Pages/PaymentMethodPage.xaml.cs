using HashGo.Core.Contracts.View;
using HashGo.Domain.ViewModels;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.Views;
using HashGo.Wpf.App.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.ApplicationModel.UserActivities;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for PaymentMethodPage.xaml
    /// </summary>
    public partial class PaymentMethodPage : BasePage
    {
        public PaymentMethodPage(PaymentMethodViewModel viewModel,IPopupService popupService):base(popupService)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            this.Loaded += (sender, e) =>
            {
                Process[] oskProcesses = Process.GetProcessesByName("TabTip");

                if (oskProcesses?.Length > 0)
                {
                    foreach (Process process in oskProcesses)
                    {
                        process.Kill();
                    }
                }
            };
        }
    }
}
