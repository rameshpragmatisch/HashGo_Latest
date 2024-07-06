using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class QRPaymentPageViewModel :BaseViewModel
    {
        INavigationService navigationService;
        DispatcherTimer timer = null;

        public QRPaymentPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            NavigateToPreviousScreenCommand = new RelayCommand(OnNavigateToPreviousScreen);
        }

        private void OnNavigateToPreviousScreen()
        {
            navigationService.NavigateToAsync(Pages.PaymentMethod.ToString());
        }

        #region Command

        public ICommand NavigateToPreviousScreenCommand { get; private set; }

        #endregion

        public override void ViewLoaded()
        {
            int defaultTimeInSecs = 10;

            if (!string.IsNullOrEmpty(HashGoAppSettings.PaymentScreenVisibleDelay))
                defaultTimeInSecs = Convert.ToInt32(HashGoAppSettings.PaymentScreenVisibleDelay);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(defaultTimeInSecs)   //Time can change
            };

            timer.Tick += (sender, e) =>
            {
                navigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
            };

            timer.Start();
        }

        public override void ViewUnloaded()
        {
            timer.Stop();
            timer = null;
        }
    }
}
