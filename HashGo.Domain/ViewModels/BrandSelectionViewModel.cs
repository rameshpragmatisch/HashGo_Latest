using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using System.Collections.ObjectModel;

namespace HashGo.Domain.ViewModels
{
    public partial class BrandSelectionViewModel : BaseNavigateableViewModel<IRestaurantBrandService>
    {
        [ObservableProperty]
        IEnumerable<RestaurantBrand> restaurants = Array.Empty<RestaurantBrand>();

        [ObservableProperty]
        private RestaurantBrand? selectedRestaurant = null;

        [ObservableProperty]
        IEnumerable<TenantConnect> tenantConnects = Array.Empty<TenantConnect>();


        public BrandSelectionViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                            INavigationService navigationService)
            : base(loggingService, brandService, navigationService)
        {
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Restaurants = new ObservableCollection<RestaurantBrand>(await this.BrandService.ReadBrandsForConnect(TenantConnects.ToArray()));

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        [RelayCommand(CanExecute = nameof(CanNavigateToRestaurantHome))]
        private async Task NavigateToRestaurantHome(RestaurantBrand restaurantBrand)
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToRestaurantHome)}() Started.");

            var parameters = new Dictionary<string, object>
            {
                { "SelectedRestaurant", restaurantBrand },
            };

            if (restaurantBrand.DepartmentCount > 1)
                await NavigationService.NavigateToAsync(Pages.DiningOption.ToString(), parameters);
            else
                await NavigationService.NavigateToAsync(Pages.RestaurantStart.ToString(), parameters);

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToRestaurantHome)}() Completed.");

        }

        private bool CanNavigateToRestaurantHome() { return this.Restaurants?.Any() == true; }

        partial void OnSelectedRestaurantChanged(RestaurantBrand? value)
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(OnSelectedRestaurantChanged)}() Started.");

            NavigateToRestaurantHomeCommand.Execute(value);

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(OnSelectedRestaurantChanged)}() Completed.");

        }
        [RelayCommand]
        private async void NavigateToConfigurationPage()
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToConfigurationPage)}() Started.");

            var navigated = await this.NavigateToPage(Pages.TenantConnectConfiguration, Array.Empty<object>());

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToConfigurationPage)}() Completed.");

        }
    }
}
