using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;

namespace HashGo.Domain.ViewModels
{
    public abstract partial class BaseResturantViewModel : BaseNavigateableViewModel<IRestaurantBrandService>
    {
        private readonly IOrderService orderService;

        [ObservableProperty]
        private long orderId;

        [ObservableProperty]
        private string orderQueueNumber;

        public BaseResturantViewModel(ILoggingService loggingService,
                                      IRestaurantBrandService brandService,
                                      INavigationService navigationService,
                                      IOrderService orderService)
            : base(loggingService, brandService, navigationService)
        {
            this.orderService = orderService;
        }

        public IOrderService OrderService { get { return orderService; } }

        public IRestaurantBrandService ResturantBrandService { get { return BrandService; } }
    }
}
