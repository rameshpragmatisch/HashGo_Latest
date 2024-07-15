using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.Services;
using HashGo.Domain.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class EnquiriesPageViewModel : BaseViewModel
    {
        INavigationService navigationService;
        IRetailConnectService retailConnectService;

        public EnquiriesPageViewModel(INavigationService navigationService,IRetailConnectService retailConnectService)
        {
            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;

            PreviousScreenCommand = new RelayCommand(OnPreviousScreenClicked);
            SubmitEnquiryCommand = new RelayCommand(OnSubmitEnquiry);
        }

        /// <summary>
        /// 
        /// </summary>
         async void OnSubmitEnquiry()
        {
            //TODO: Service method to raise a enquiry
             var result =await retailConnectService.CreateEnquiryRequest(new Core.Models.EnquiriesRequestObject()
            {
                  enquiry = new Core.Models.EnquiryRequest()
                  {
                       name = this.Name,
                        message = this.Enquiries,
                         phoneNo = this.PhoneNumber,
                  }
            });

            if (result)
            {
                var parameters = new Dictionary<string, object>
            {
                { "IsEnquiry", true },
            };

                //Navigate to EnquiriesConfirmed screen 
                navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString(), parameters);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OnPreviousScreenClicked()
        {
            navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
        }

        #region Properties

        string enquiries;

        public string Enquiries 
        {
            get => enquiries;
            set
            {
                enquiries = value;
                IsEnabled = !string.IsNullOrEmpty(enquiries) &&
                            !string.IsNullOrEmpty(phoneNumber) &&
                            phoneNumber.Length == 8 &&
                            !string.IsNullOrEmpty(name);
                OnPropertyChanged();
            }
        }

        string phoneNumber;
        public string PhoneNumber 
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                IsEnabled = !string.IsNullOrEmpty(enquiries) &&
                            !string.IsNullOrEmpty(phoneNumber) &&
                            phoneNumber.Length == 8 &&
                            !string.IsNullOrEmpty(name);
                OnPropertyChanged();
            }
        }

        string name;

        public string Name 
        {
            get => name;
            set
            {
                name = value;
                IsEnabled = !string.IsNullOrEmpty(enquiries) &&
                            !string.IsNullOrEmpty(phoneNumber) &&
                            phoneNumber.Length == 8 &&
                            !string.IsNullOrEmpty(name);
                OnPropertyChanged();
            }
        }


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

        #endregion

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand SubmitEnquiryCommand { get; private set; }
        

        #endregion
        public override void ViewLoaded()
        {
            this.Enquiries = null;
            this.PhoneNumber = null;
            this.Name = null;
        }
    }
}
