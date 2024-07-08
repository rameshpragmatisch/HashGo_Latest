using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using HashGo.Infrastructure.Models;
using HashGo.Wpf.App.BestTech.Views;
using HashGo.Wpf.App.Models.BestTech;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class ProductSelectionPageViewModel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly INavigationService navigationService;
        readonly IRetailConnectService retailConnectService;
        readonly IEventAggregator eventAggregator;
        SharedDataService sharedDataService = null;

        public ProductSelectionPageViewModel(ILoggingService loggingService,
                                             INavigationService navigationService,
                                             IRetailConnectService retailConnectService,
                                             IEventAggregator eventAggregator,
                                             SharedDataService sharedDataService)
        {
            logger = loggingService;
            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;
            this.eventAggregator = eventAggregator;
            this.sharedDataService = sharedDataService;

            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
            StartOverScreenCommand = new RelayCommand(OnMoveToStartScreen);
            ViewCartCommand = new RelayCommand<Point>(OnMoveToViewCartScreen);
            UnitsSelectionChangedCommand = new RelayCommand<object>(OnUnitsSelectionChanged);
            PreviousScreenCommand = new RelayCommand(OnMoveToPreviousScreen);

            //eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Subscribe(OnClearData);
        }

        void OnMoveToPreviousScreen()
        {
            navigationService.NavigateToAsync(Pages.CustomerDetails.ToString());
        }

        private void OnClearData(bool isClearData)
        {
            SelectedUnit = null;
            SelectedCategory = this.Categories?.FirstOrDefault();
            sharedDataService.ClearData();
            sharedDataService.ClearCustomerData();
            sharedDataService.CustomerDetailsObj = new CustomerDetails();
            //SelectedUnits = new List<Unit>();
        }

        async Task InitializeDataAsync()
        {
            logger.Trace($"{nameof(ProductSelectionPageViewModel)} : {nameof(InitializeDataAsync)}() Started.");
            var lstCategories = await Task.Run(() => LoadCategories());

            if(lstCategories != null && lstCategories.Count > 0)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.categories = lstCategories;
                    SelectedCategory = categories.First();
                    OnPropertyChanged(nameof(Categories));
                });
            }
           
            logger.Trace($"{nameof(ProductSelectionPageViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        List<UICategory> LoadCategories()
        {
            logger.Trace($"{nameof(ProductSelectionPageViewModel)} : {nameof(LoadCategories)}() Started.");

            try
            {
                var temp = this.retailConnectService.GetCategoryiesByDepartmentId(ApplicationStateContext.DepartmentId)
                                                      .Result;
                var lst = temp.Where(ee=>Convert.ToString(ee.name).ToUpper() != "ADD ONS")
                                                      .Select(ee => new UICategory(ee.name,
                                                       ee.code,
                                                       (ee.categoryImage == null) ?
                                                       CommonConstants.DEFAULTIMAGE : Convert.ToString(ee.categoryImage),
                                                       ee.monthlyQtyLimit,
                                                       ee.id)).ToList();

                var addonCategory = temp.FirstOrDefault(ee => Convert.ToString(ee.name).ToUpper() == "ADD ONS");

                if(addonCategory != null)
                {
                    sharedDataService.AddOnId = addonCategory.id;
                }

                return lst;

                //if (categories != null && categories.Count > 0)
                //{
                //    SelectedCategory = categories.First();
                //}

                //OnPropertyChanged(nameof(Categories));
            }
            catch( Exception ex )
            {
                logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(LoadCategories)} {ex.Message}");
                IsBusy = false;
                return new List<UICategory>() ;
            }

            return new List<UICategory>();

            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(LoadCategories)}() Completed.");
        }

        List<UISubCategory> LoadSubCategories()
        {
            logger.Trace($"{nameof(ProductSelectionPageViewModel)} : {nameof(LoadSubCategories)}() Started.");

            try
            {
                return this.retailConnectService.GetSubCategoriesByCategoryId(SelectedCategory.Id)
                                                           .Result.Where(xx => xx.categoryId == SelectedCategory.Id)
                                                           .Select(ee => new UISubCategory(ee.name,
                                                            ee.code,
                                                            ee.categoryId,
                                                            ee.categoryName,
                                                            ee.id)).ToList();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                return new List<UISubCategory>();
            }

            logger.Trace($"{nameof(RestaurantStartupPageViewModel)} : {nameof(LoadSubCategories)}() Completed.");
        }

        void OnUnitsSelectionChanged(object unit)
        {
            if (SelectedUnit == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "CanAddItem", true },
            };
            //sharedDataService.AddItem(this.SelectedUnit);
            navigationService.NavigateToAsync(Pages.Addons.ToString(), parameters);  //, parameters
        }

        /// <summary>
        /// Move to Payment screen
        /// </summary>
        void OnMoveToViewCartScreen(Point point)
        {
            if (point == default(Point)) return;

            ViewCartPopup viewCartPopup = new ViewCartPopup(point);
            viewCartPopup.Owner = Application.Current.MainWindow;
            viewCartPopup.DataContext = new ViewCartPopupViewModel(sharedDataService, navigationService);
            viewCartPopup.Show();
        }

        /// <summary>
        /// Move to Addons screen.
        /// </summary>
        void OnMoveToNextScreen()
        {
            //var parameters = new Dictionary<string, object>
            //    {
            //        { nameof(SelectedUnits), SelectedUnits },
            //    };
            
            navigationService.NavigateToAsync(Pages.Addons.ToString());  //parameters
        }

        public override void ViewLoaded()
        {
            InitializeDataAsync();
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Unsubscribe(OnClearData);
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Subscribe(OnClearData);
        }

        public override void ViewUnloaded()
        {
            //eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Unsubscribe(OnClearData);
        }

        /// <summary>
        /// Navigate to home screen.
        /// </summary>
        void OnMoveToStartScreen()
        {
            //Confirm first
            ConfirmPopup confirmPopup = new ConfirmPopup();
            var diaglogResult = confirmPopup.ShowDialog();

            if (diaglogResult.Value)
            {
                this.OnClearData(true);
                navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
            }
        }

        //void OnMoveToStartScreen()
        //{
        //    //var parameters = new Dictionary<string, object>
        //    //{
        //    //    { "CanAddItem", true },
        //    //};
        //    //navigationService.NavigateToAsync(Pages.Addons.ToString(), parameters);  //, parameters

        //    navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
        //}

        #region Commands

        public ICommand StartOverScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }
        public RelayCommand<Point> ViewCartCommand { get; private set; }
        public RelayCommand<object> UnitsSelectionChangedCommand { get; private set; }
        public ICommand PreviousScreenCommand { get; private set; }

        #endregion

        #region Properties

        bool isBusy = false;
        public bool IsBusy 
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        List<UICategory> categories = new List<UICategory>();

        public ReadOnlyCollection<UICategory> Categories { get { return categories.AsReadOnly(); } }

        UICategory selectedCategory = null;
        public UICategory SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;

                if (value != null)   //Get the sub categories
                {
                    IsBusy = true;
                    LoadSubCategoriesAsync();
                }

                OnPropertyChanged();
            }
        }

        async Task LoadSubCategoriesAsync()
        {
            var lstSubCategories = await Task.Run(() => LoadSubCategories());

            if(lstSubCategories != null && lstSubCategories.Count > 0)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    SelectedCategory.SetSubCategories(lstSubCategories);
                    SelectedSubCategory = SelectedCategory.LstUISubCategories.First();
                });
            }
        }

        UISubCategory selectedSubCategory = null;
        public UISubCategory SelectedSubCategory
        {
            get => selectedSubCategory;
            set
            {
                selectedSubCategory = value;

                if (value != null)   //Get the Products
                {
                    //Call service method to load products
                    LoadProductsAsync();
                     IsBusy = false;
                }

                OnPropertyChanged();
            }
        }

        async Task LoadProductsAsync()
        {
            var products = await Task.Run(() => LoadProducts());

            if(products !=null && products.Count>0)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    SelectedSubCategory?.SetProducts(products);
                });
            }
        }

        List<Unit> LoadProducts()
        {
            try
            {
                return this.retailConnectService.GetProductsByCategoryAndSubcategoryId(selectedCategory.Id, selectedSubCategory.Id)
                                                     .Result
                                                     .Select(ee => new Unit(ee.unitId, ee.id, ee.unitName, ee.name,
                                                                                string.IsNullOrEmpty(ee.imagePath) ?
                                                      CommonConstants.DEFAULTIMAGE : ee.imagePath,
                                                      ee.price,
                                                      ee.remarks)).ToList();
            }
            catch(Exception ex)
            {
                return new List<Unit>();
            }
        }

        Unit selectedUnit;

        public Unit SelectedUnit
        {
            get => sharedDataService.SelectedUnit;
            set
            {
                sharedDataService.SelectedUnit = value;
                OnPropertyChanged();

                //if(value != null)
                //   {
                //        if (SelectedUnits.Count > 0)
                //        {
                //            var alreadyExistingItem = SelectedUnits.FirstOrDefault(ee => ee.Id == selectedUnit.Id);

                //            if (alreadyExistingItem != null)
                //                alreadyExistingItem.UnitCount++;
                //            else SelectedUnits.Add(SelectedUnit);
                //        }
                //        else SelectedUnits.Add(SelectedUnit);
                //  }
            }
        }

        public List<Unit> SelectedUnits
        {
            get
            {
                return this.sharedDataService.SelectedUnits;
            }
            set
            {
                this.sharedDataService.SelectedUnits = value;
            }
        }

        #endregion
    }

   
}
    
