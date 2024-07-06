using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public  class ConfirmDineDatePageViewModel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly INavigationService navigationService;
        readonly IEventAggregator eventAggregator;
        readonly SharedDataService sharedDataService;

        public ConfirmDineDatePageViewModel(ILoggingService loggingService,
                                            INavigationService navigationService,
                                            IEventAggregator eventAggregator,
                                            SharedDataService sharedDataService)
        {
            this.sharedDataService = sharedDataService;
            this.logger = loggingService;
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;

            PreviousScreenCommand = new RelayCommand(OnPreviousScreenClicked);
            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
        }

        void OnClearData(bool isClearData)
        {
             SelectedDate = DateTime.Now;
        }

        void OnMoveToNextScreen()
        {
            ApplicationStateContext.CustomerDate = SelectedDate.Date;
            ApplicationStateContext.IsMorningTime = this.isMorningSelected;
            ApplicationStateContext.IsEveningTime = this.isEveningSelected;
            navigationService.NavigateToAsync(Pages.CustomerDetails.ToString());
        }

        void OnPreviousScreenClicked()
        {
            navigationService.NavigateToAsync(Pages.Payment.ToString());
        }

        public override void ViewLoaded()
        {
            this.IsMorningSelected = ApplicationStateContext.IsMorningTime;
            this.IsEveningSelected = ApplicationStateContext.IsEveningTime;
            //IsServiceDepartment = (ApplicationStateContext.DepartmentId == 706);
            //SelectedDate = DateTime.Now;
            //eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Subscribe(OnClearData);
            this.SelectedDate = sharedDataService.CustomerDateTime;
        }

        public override void ViewUnloaded()
        {
            //eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Unsubscribe(OnClearData);
        }

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }

        #endregion

        #region Properties

        DateTime selectedDate = DateTime.Today.Date;
        public DateTime SelectedDate
        {
            get => sharedDataService.CustomerDateTime;
            set
            {
                sharedDataService.CustomerDateTime = value.Date;
                OnPropertyChanged();
            }
        }

        bool isServiceDepartment;
        public bool IsServiceDepartment 
        {
            get => isServiceDepartment;
            set
            {
                isServiceDepartment = value;
                OnPropertyChanged();
            }
        }

        bool isMorningSelected;
        public bool IsMorningSelected 
        {
            get => isMorningSelected;
            set
            {
                isMorningSelected = value;

                if (value)
                    IsEveningSelected = false;
                OnPropertyChanged();
            }
        }

        bool isEveningSelected;
        public bool IsEveningSelected
        {
            get => isEveningSelected;
            set
            {
                isEveningSelected = value;

                if (value)
                    IsMorningSelected = false;

                OnPropertyChanged();
            }
        }



        #endregion
    }
}
