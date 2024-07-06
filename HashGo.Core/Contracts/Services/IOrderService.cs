using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Core.Contracts.Services
{
    public interface IOrderService : IDomainService
    {
        Task<long> StartNewOrderAsync();

        Task SetOrderDiningOptionAsync(long orderId, DiningOption diningOption);

        Task<IReadOnlyCollection<PaymentMethod>> ReadPaymentMethod();

        string GenerateFlyTicket(CartItem cartItem, RestaurantBrand restaurantBrand);
    }
}
