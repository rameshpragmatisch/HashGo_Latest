using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Models;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Enum;
using System.ComponentModel;
using System.Drawing;

namespace HashGo.Domain.ViewModels
{
    public partial class BrandMenuViewModel : BaseWorkFlowViewModel, INotifyPropertyChanged
    {
        IViewService viewService;

        [ObservableProperty]
        IEnumerable<ScreenMenu> menus = Array.Empty<ScreenMenu>();

        [ObservableProperty]
        IEnumerable<ScreenCategory> categories = Array.Empty<ScreenCategory>();

        [ObservableProperty]
        IEnumerable<MenuItem> menuItems = Array.Empty<MenuItem>();

        [ObservableProperty]
        ScreenMenu selectedMenu;

        [ObservableProperty]
        ScreenCategory selectedCategory;

        [ObservableProperty]
        IView mainView;

        public static int LastSelectedItemIndex;


       



        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                LastSelectedItemIndex = SelectedIndex;
            }
        }

        public BrandMenuViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                       INavigationService navigationService,
                                       IOrderService orderService,
                                       IViewService viewService)
            : base(loggingService, brandService, navigationService, orderService)
        {
            this.viewService = viewService;
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();
            if (this.Menus.Any())
            {
                
                this.SelectedMenu = this.Menus.First();
                if (this.SelectedMenu?.Categories?.Any() == true)
                {
                    if (LastSelectedItemIndex > 0)
                    {
                        SelectedIndex = LastSelectedItemIndex;
                    }
                    else
                    {
                        this.SelectedCategory = this.SelectedMenu?.Categories.First();
                    }

                }
            }

            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Menus = await this.ResturantBrandService.ReadMenuAsync(this.SelectedRestaurant);

            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(LoadDataAsync)}() Completed.");

        }

        private bool CanProcessMenuItem(MenuItem menuItem) { return !this.IsLoading && menuItem != null; }


        [RelayCommand(CanExecute = nameof(CanProcessMenuItem))]
        private async Task ProcessMenuItem(MenuItem menuItem)
        {
            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(ProcessMenuItem)}() Started.");

            this.SelectedMenuItem = menuItem;

            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                { nameof(this.SelectedMenuItem), menuItem },
            };

            if (menuItem.IsShowComboText)
                await NavigationService.NavigateToAsync(Pages.MealItSelection.ToString(), parameters);
            else
            {
                this.SelectedMealItOption = MealItOptions.AlaCarte;
                await NavigateToMenuDetails(Pages.MealItWorkFlow);
            }

            this.Logger.Trace($"{nameof(BrandMenuViewModel)} : {nameof(ProcessMenuItem)}() Completed.");

        }

        private async Task NavigateToMenuDetails(Pages page)
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Started.");

            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                { nameof(this.SelectedMenuItem), this.SelectedMenuItem },
                { nameof(this.SelectedMealItOption), this.SelectedMealItOption },
            };

            await NavigationService.NavigateToAsync(page.ToString(), parameters);

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Completed.");

        }



    }
}
