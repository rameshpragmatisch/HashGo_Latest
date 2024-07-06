using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Core.Contracts.Services
{
    public interface IRestaurantBrandService : IDomainService
    {
        Task<IReadOnlyCollection<RestaurantBrand>> ReadAllBrands();
        Task<IReadOnlyCollection<RestaurantBrand>> ReadBrandsForConnect(IReadOnlyCollection<TenantConnect> connectItems);
        Task<IReadOnlyCollection<ScreenMenu>> ReadMenuAsync(RestaurantBrand brand);
        Task<TimeSpan> ReadEstimatedWaitTimeAsync(long resturantId);
        Task<WorkFlow> ReadWorkFlowAsync(MenuItem selectedMenu, string restaurantUniqueId, long menuItemId, DiningOption orderDiningOption, MealItOptions mealItOption);
        Task<string> ReadBrandEstimateTimeAsync(RestaurantBrand selectedRestaurant);
    }
}
