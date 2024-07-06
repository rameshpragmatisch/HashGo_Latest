using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;

namespace HashGo.Domain.ViewModels
{
    public partial class ItemAddedToCartViewModel : BaseResturantViewModel
    {
        [ObservableProperty]
        Pages pageToNavigate;

        [ObservableProperty]
        decimal amount;
        
        public ItemAddedToCartViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                        INavigationService navigationService,
                                        IOrderService orderService) 
            : base(loggingService, brandService, navigationService, orderService)
        {
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            await NavigateToPage();

            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(LoadDataAsync)}() Started.");


            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private async Task NavigateToPage()
        {
            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(NavigateToPage)}() Started.");

            await Task.Delay(4000);
            
            await this.NavigateToPage(this.PageToNavigate, Array.Empty<object>());

            this.Logger.Trace($"{nameof(ItemAddedToCartViewModel)} : {nameof(NavigateToPage)}() Completed.");
        }
    }
}
