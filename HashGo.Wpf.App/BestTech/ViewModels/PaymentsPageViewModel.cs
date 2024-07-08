using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Domain.Services;
using HashGo.Domain.ViewModels;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.Events;
using HashGo.Wpf.App.BestTech.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class PaymentsPageViewModel : BaseViewModel
    {
        ILoggingService logger;
        INavigationService navigationService;
        IOrderService orderService;
        INetworkService networkService;
        IEventAggregator eventAggregator;
        SharedDataService sharedDataService;
        readonly IPopupService popupService;

        public PaymentsPageViewModel(ILoggingService loggingService,
                                     INavigationService navigationService, 
                                     IOrderService orderService, 
                                     INetworkService networkService,
                                     IEventAggregator eventAggregator,
                                     SharedDataService sharedDataService,
                                     IPopupService popupService)
        {
            this.sharedDataService = sharedDataService;
            logger = loggingService;
            this.navigationService = navigationService;
            this.orderService = orderService;
            this.networkService = networkService;
            this.eventAggregator = eventAggregator;
            this.popupService = popupService;

            StartOverCommand = new RelayCommand(OnStartOver);
            OrderMoreCommand = new RelayCommand(OnOrderMore);
            ClearCartCommand = new RelayCommand(OnClearCartSelected);
            RemoveProductCommand = new RelayCommand<Unit>(OnRemoveClicked);
            AddProductCommand = new RelayCommand(OnAddClicked);
            ProceedToPaymentsCommand = new RelayCommand(OnProceedToPayments);
            EditProductCommand = new RelayCommand<Unit>(OnEditProduct);
            RemoveUnitCommand= new RelayCommand<Unit>(OnRemoveUnitClicked);
            AddRefferalCodeCommand = new RelayCommand(OnAddRefferalCode);
        }

        void OnAddRefferalCode()
        {
            bool isValid = popupService.ShowPopup(Core.Contracts.Enums.PopupType.eRefferalCode);

            if(isValid)
            {
                this.ReferralCode = sharedDataService.RefferalCode;
            }
        }

        void OnRemoveUnitClicked(Unit unit)
        {
            if (unit == null)
                return;

            ConfirmPopup confirmPopup = new ConfirmPopup(message: "Item will be removed from cart\nDo you want to proceed?");
            confirmPopup.Owner = Application.Current.MainWindow;
            var diaglogResult = confirmPopup.ShowDialog();

            if (diaglogResult.Value)
            {
                SelectedUnits.Remove(unit);
                SelectedUnits = new List<Unit>(SelectedUnits);
            }
        }

        void OnProceedToPayments()
        {
            //bool canProceedFurthur = popupService.ShowPopup(Core.Contracts.Enums.PopupType.eConfirmCustomerDetails);

            var parameters = new Dictionary<string, object>
                {
                    { "IsServiceDepartment", sharedDataService.DepartmentName?.ToUpper() == "SERVICING" },
                };
            navigationService.NavigateToAsync(Pages.DineDateSelect.ToString(), parameters);
        }

        public bool IsNavigatetoConfirmCustomerDetailsPopup { get; set; }

        void OnAddClicked()
        {
            CalculateTotalPrice();
        }
        void CalculateTotalPrice()
        {
            this.TotalPrice = sharedDataService.SelectedUnits?.Sum(ee => ((ee.UnitPrice+ee.AddOnsPrice) * ee.UnitCount));
        }

        void OnRemoveClicked(Unit unit)
        {
            if (unit == null)
                return;

            if(unit.UnitCount == 0)
                SelectedUnits.Remove(unit);

            SelectedUnits = new List<Unit>(SelectedUnits);

            CalculateTotalPrice();
        }

        /// <summary>
        /// this method clears the selected Products.
        /// </summary>
        void OnClearCartSelected()
        {
            if (SelectedUnits == null || SelectedUnits.Count == 0)
                return;

            ConfirmPopup confirmPopup = new ConfirmPopup(message: "Do you want to clear the Cart?");
            var diaglogResult = confirmPopup.ShowDialog();

            if (diaglogResult.Value)
            {
                SelectedUnit = null;
                SelectedUnits = new List<Unit>();
                sharedDataService.ClearData();
                sharedDataService.ClearCustomerData();
                navigationService.NavigateToAsync(Pages.ProductSelection.ToString());
            }
        }

        /// <summary>
        /// this method navigates to the start screen.
        /// </summary>
        void OnStartOver()
        {
            ConfirmPopup confirmPopup = new ConfirmPopup();
            var diaglogResult = confirmPopup.ShowDialog();

            if (diaglogResult.Value) 
            {
                SelectedUnit = null;
                SelectedUnits = new List<Unit>();
                sharedDataService.ClearCustomerData();
                navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
            }  
        }

        public override void ViewLoaded()
        {
            SelectedUnits = new List<Unit>(sharedDataService.SelectedUnits);
            OnPropertyChanged(nameof(SelectedUnits));
            this.ReferralCode = sharedDataService?.RefferalCode;

            if(IsNavigatetoConfirmCustomerDetailsPopup)
            {
                IsNavigatetoConfirmCustomerDetailsPopup = false;
                this.OnProceedToPayments();
            }
        }

        void OnEditProduct(Unit unit)
        {
            if (unit == null)
                return;

            sharedDataService.SelectedUnit = null;
            sharedDataService.SelectedUnit = unit;

            var parameters = new Dictionary<string, object>
            {
                { "CanAddItem", false },
                {"IsNavigateToPaymentsPage",true }
            };
            navigationService.NavigateToAsync(Pages.Addons.ToString(), parameters);   //parameters
        }

        void OnOrderMore()
        {
            sharedDataService.SelectedUnit = null;
            var parameters = new Dictionary<string, object>
                {
                    { nameof(SelectedUnit), null },
                };
           
            navigationService.NavigateToAsync(Pages.ProductSelection.ToString(), parameters);
        }

        Unit selectedUnit;

        public Unit SelectedUnit 
        {
            get => sharedDataService.SelectedUnit;
            set
            {
                sharedDataService.SelectedUnit = value;

                //if (value != null)
                //{
                //    if (SelectedUnits.Count > 0)
                //    {
                //        var alreadyExistingItem = SelectedUnits.FirstOrDefault(ee => ee.Id == selectedUnit.Id);

                //        if (alreadyExistingItem != null)
                //            alreadyExistingItem.UnitCount++;
                //        else SelectedUnits.Add(SelectedUnit);
                //    }
                //    else SelectedUnits.Add(SelectedUnit);

                //    SelectedUnits = new List<Unit>(SelectedUnits);
                //}
            }
        }

        List<Unit> selectedUnits = new List<Unit>();

        public List<Unit> SelectedUnits 
        {
            get => sharedDataService.SelectedUnits;
            set
            {
                sharedDataService.SelectedUnits = value;

                TotalPrice = null;

                if (value?.Count > 0)
                {
                    CalculateTotalPrice();
                }

                OnPropertyChanged();
            }
        }

        string refferalCode;
        public string ReferralCode
        {
            get => refferalCode;
            set
            {
                refferalCode = value;
                OnPropertyChanged();
            }
        }

        double? totalPrice;
        public double? TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;

                if(totalPrice != null)
                {
                    Deposit = totalPrice / 5;
                }
                OnPropertyChanged();
            }
        }

        double? deposit;

        public double? Deposit 
        {
            get => deposit;
            set
            {
                deposit = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        public ICommand StartOverCommand { get; private set; }
        public ICommand OrderMoreCommand { get; private set; }
        public ICommand ProceedToPaymentsCommand { get; private set; }
        public ICommand ClearCartCommand { get; private set; }
        public RelayCommand<Unit> RemoveProductCommand { get; private set; }
        public ICommand AddProductCommand { get; private set; }

        public RelayCommand<Unit> EditProductCommand { get; set; }
        public RelayCommand<Unit> RemoveUnitCommand { get; private set; }
        public ICommand AddRefferalCodeCommand { get; private set; }
        
        #endregion
    }
}
