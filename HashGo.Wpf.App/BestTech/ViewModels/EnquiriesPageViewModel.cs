using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
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

        public EnquiriesPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            PreviousScreenCommand = new RelayCommand(OnPreviousScreenClicked);
            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
        }

        /// <summary>
        /// 
        /// </summary>
        void OnMoveToNextScreen()
        {
            //TODO: Service method to raise a enquiry

            var parameters = new Dictionary<string, object>
            {
                { "IsEnquiry", true },
            };

            //Navigate to EnquiriesConfirmed screen 
            navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString(), parameters);
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
                IsEnabled = !string.IsNullOrEmpty(enquiries) && !string.IsNullOrEmpty(phoneNumber);
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
                IsEnabled = !string.IsNullOrEmpty(enquiries) && !string.IsNullOrEmpty(phoneNumber);
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
        public ICommand NextScreenCommand { get; private set; }

        #endregion
        public override void ViewLoaded()
        {
            this.Enquiries = null;
            this.PhoneNumber = null;
        }
    }
}
