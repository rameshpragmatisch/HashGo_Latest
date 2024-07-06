using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using HashGo.Infrastructure.Services;
using HashGo.Wpf.App.BestTech.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class RestaurantStartupPageViewModel : BaseViewModel
    {
        ILoggingService logger;
        INavigationService navigationService;
        IRetailConnectService retailConnectService;
        SharedDataService sharedDataService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="navigationService"></param>
        public RestaurantStartupPageViewModel(ILoggingService loggingService,
                                              INavigationService navigationService, 
                                              IRetailConnectService retailConnectService,
                                              SharedDataService sharedDataService)
        {
            this.sharedDataService = sharedDataService;
            logger = loggingService;
            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;
            InitializeDataAsync();

            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
            NavigateToEnquiriesCommand = new RelayCommand(OnNavigateToEnquiries);
            NavigateToStoreLocatorsCommand = new RelayCommand(OnNavigateToStoreLocators);
            NavigateToSettingsScreenCommand = new RelayCommand(OnNavigateToSettingsScreen);
            this.sharedDataService = sharedDataService;
        }

        void OnNavigateToSettingsScreen()
        {
            navigationService.NavigateToAsync(Pages.HashGoSettings.ToString());
        }

        /// <summary>
        /// navigates to store locators view
        /// </summary>
        void OnNavigateToStoreLocators()
        {
            StoreLocatorsPopup storeLocatorsPopup = new StoreLocatorsPopup();
            storeLocatorsPopup.DataContext = new StoreLocatorsPopupViewModel();
            storeLocatorsPopup.ShowDialog();
        }

        /// <summary>
        /// navigate to enquiries page
        /// </summary>
        void OnNavigateToEnquiries()
        {
            navigationService.NavigateToAsync(Pages.Enquiries.ToString());
        }

        /// <summary>
        /// this method navigates to next screen
        /// </summary>
        void OnMoveToNextScreen()
        {

            //This code is commented intentionally dont delete
            //var selectedProduct = lstProducts.FirstOrDefault(ee => ee.IsSelected == true);

            // if(selectedProduct != null)
            // {

            // }
            var selectedDepartment = LstDepartments.First(ee => ee.IsSelected == true);
            ApplicationStateContext.DepartmentId = selectedDepartment.Id;
            //var parameters = new Dictionary<string, object>
            //    {
            //        { "IsServiceDepartment", selectedDepartment.DepartmentName.ToUpper() == "SERVICING" },
            //    };
            //navigationService.NavigateToAsync(Pages.DineDateSelect.ToString(), parameters );

            sharedDataService.DepartmentName = selectedDepartment.DepartmentName;
            navigationService.NavigateToAsync(Pages.ProductSelection.ToString());
        }

        #region Properties

        ObservableCollection<DepartmentModel> lstDepartments = new ObservableCollection<DepartmentModel>();
        public ObservableCollection<DepartmentModel> LstDepartments
        {
            get => lstDepartments;
            set
            {
                lstDepartments = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand NextScreenCommand { get; private set; }
        public ICommand NavigateToEnquiriesCommand { get; private set; }
        public ICommand NavigateToStoreLocatorsCommand { get; private set; }

        public ICommand NavigateToSettingsScreenCommand { get; private set; }

        #endregion

        async Task InitializeDataAsync()
        {
            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(InitializeDataAsync)}() Started.");
            await this.LoadDataAsync();
            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        async Task LoadDataAsync()
        {
            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(LoadDataAsync)}() Started.");

            var departments = this.retailConnectService.GetAllDepartments().Result;
            //var products = await productDetailStoreService.ReadAllAsync();
            LstDepartments = new ObservableCollection<DepartmentModel>(departments.Select(ee => new DepartmentModel(ee.name, ee.id)));

            foreach (var item in LstDepartments)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }

            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is DepartmentModel departmentModel)
            {
                foreach (var item in LstDepartments)
                {
                    if (item != departmentModel)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                        item.IsSelected = false;
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// this method clears all the properties of a class.
        /// </summary>
        public override void ViewLoaded()
        {
            if (lstDepartments == null || lstDepartments.Count == 0)
                this.LoadDataAsync();

            if (lstDepartments == null || lstDepartments.Count == 0)
                return;

                foreach (var product in LstDepartments)
            {
                product.IsSelected = false;
            }

            ApplicationStateContext.ClearData();
            sharedDataService.ClearData();
            sharedDataService.ClearCustomerData();
            //ApplicationStateContext.CustomerDate = null;
        }
    }

    public class DepartmentModel : INotifyPropertyChanged
    {
        string departmentName;

        public string DepartmentName
        {
            get => departmentName;
            set
            {
                departmentName = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        bool isSelected;

        public DepartmentModel(string departmentName, int id)
        {
            DepartmentName = departmentName;
            Id = id;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
