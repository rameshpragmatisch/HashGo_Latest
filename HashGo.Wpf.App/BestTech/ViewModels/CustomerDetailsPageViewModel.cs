using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using HashGo.Infrastructure.Models;
using HashGo.Wpf.App.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class CustomerDetailsPageViewModel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly INavigationService navigationService;
        readonly IEventAggregator eventAggregator;
        SharedDataService sharedDataService;
        IPopupService popupService;

        public CustomerDetailsPageViewModel(ILoggingService loggingService,
                                            INavigationService navigationService,
                                            IEventAggregator eventAggregator,
                                            SharedDataService sharedDataService,
                                            IPopupService popupService)
        {
            this.sharedDataService = sharedDataService;
            logger = loggingService;
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.popupService = popupService;

            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);  //,CanMoveToNextScreen
            PreviousScreenCommand = new RelayCommand(OnMoveBackToPreviousScreen);

            
        }

        void SetIsEnabled()
        {
            IsEnabled = !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.Name) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.ContactNumber) &&
                   sharedDataService.CustomerDetailsObj.PostalCode != null &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.UnitNo) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.FloorNo) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.AddressLine1);
        }

        void OnClearData(bool isClearData)
        {
            CustomerDetailsObj = new CustomerDetails();
            CustomerDetailsObj.PropertyChanged += (sender, args) =>
            {
                SetIsEnabled();
            };
        }

        void OnMoveToNextScreen()
        {
            //Write the customer object to applicationContext
            //IsEnabled = false;
            ApplicationStateContext.CustomerDetailsObj = this.CustomerDetailsObj;

            //if(IsNavigateToConfirmCustomerDetailsScreen)
            //{
            //    IsNavigateToConfirmCustomerDetailsScreen = false;

            //    var parameters = new Dictionary<string, object>
            //        {
            //            { "IsNavigatetoConfirmCustomerDetailsPopup", true },
            //            //{ nameof(ReferralCode), },
            //        };
            //    navigationService.NavigateToAsync(Pages.Payment.ToString(), parameters);
            //}
            //else
            //{
            //    navigationService.NavigateToAsync(Pages.ProductSelection.ToString());
            //}

            bool canProceedFurthur = popupService.ShowPopup(Core.Contracts.Enums.PopupType.eConfirmCustomerDetails);


        }

        void OnMoveBackToPreviousScreen()
        {
            navigationService.NavigateToAsync(Pages.DineDateSelect.ToString());
        }

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }

        #endregion

        #region Properties

        public CustomerDetails CustomerDetailsObj { get => sharedDataService.CustomerDetailsObj; set { sharedDataService.CustomerDetailsObj = value; OnPropertyChanged(); } }

       

        bool isEnabled = false;
        public bool IsEnabled 
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsNavigateToConfirmCustomerDetailsScreen { get; set; }

        #endregion

        public override void ViewLoaded()
        {
            CustomerDetailsObj = sharedDataService.CustomerDetailsObj;

            CustomerDetailsObj.PropertyChanged += (sender, args) =>
            {
                SetIsEnabled();
            };
            SetIsEnabled();
        }
    }

    
}
