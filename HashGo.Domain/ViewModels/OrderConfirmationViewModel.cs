using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.DataContext;
using HashGo.Infrastructure.DataContext;

namespace HashGo.Domain.ViewModels
{
    public partial class OrderConfirmationViewModel : BaseResturantViewModel
    {
        IPopupService popupService;
        public OrderConfirmationViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                          INavigationService navigationService,
                                          IOrderService orderService,
                                          IPopupService popupService)
            : base(loggingService, brandService, navigationService, orderService)
        {
            this.popupService = popupService;
        }

        [ObservableProperty]
        private string orderQueueNumber;

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            OrderQueueNumber = ApplicationStateContext.OrderQueue;

            await this.LoadDataAsync();

            this.NavigateToPage();

            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            OrderQueueNumber = ApplicationStateContext.OrderQueue;

            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private async Task NavigateToPage()
        {
            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(NavigateToPage)}() Started.");

            await popupService.ShowPopupAsync(TransactionNumber);

            await Task.Delay(2000);

            //await this.NavigateToPage(Pages.RestaurantSelection, Array.Empty<object>());
            this.NavigateToPage(Pages.RestaurantStartup, null);

            this.Logger.Trace($"{nameof(OrderConfirmationViewModel)} : {nameof(NavigateToPage)}() Completed.");

        }

        public string TransactionNumber { get; set; }
    }
}
