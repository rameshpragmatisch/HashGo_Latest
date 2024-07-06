using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Domain.ViewModels
{
    public abstract partial class BaseWorkFlowViewModel : BaseResturantViewModel
    {
        [ObservableProperty]
        MenuItem selectedMenuItem;
        
        [ObservableProperty]
        MealItOptions selectedMealItOption;
        
        protected BaseWorkFlowViewModel(ILoggingService loggingService,
                                        IRestaurantBrandService brandService,
                                        INavigationService navigationService,
                                        IOrderService orderService)
            : base(loggingService, brandService, navigationService, orderService)
        {
        }
    }
}
