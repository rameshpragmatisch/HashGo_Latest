using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;

namespace HashGo.Domain.ViewModels
{
    public partial class BrandStartViewModel : BaseResturantViewModel
    {
        [ObservableProperty]
        private string restaurantEstimatedWaitTime = null;
        private IRestaurantBrandService _iRestaurantBrandService;

        public BrandStartViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                        INavigationService navigationService,
                                        IOrderService orderService) 
            : base(loggingService, brandService, navigationService, orderService)
        {
            _iRestaurantBrandService = brandService;
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(LoadDataAsync)}() Started.");

            if (this.SelectedRestaurant != null)
            {
                this.RestaurantEstimatedWaitTime = await _iRestaurantBrandService.ReadBrandEstimateTimeAsync(SelectedRestaurant);
            }

            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private bool CanNavigateToOrderHome() { return !this.IsInitializing && !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanNavigateToOrderHome))]
        private async Task NavigateToOrderHome()
        {
            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(NavigateToOrderHome)}() Started.");

            this.OrderId = await this.OrderService.StartNewOrderAsync();

            var restaurantBrand = this.SelectedRestaurant;
            if (restaurantBrand != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { nameof(this.OrderId), this.OrderId },
                    { nameof(SelectedRestaurant), restaurantBrand },
                };

                await this.NavigateToPage(Pages.RestaurantHome, parameters);
            }

            this.Logger.Trace($"{nameof(BrandStartViewModel)} : {nameof(NavigateToOrderHome)}() Completed.");
        }
    }
}
