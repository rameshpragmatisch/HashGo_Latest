using HashGo.Core.Enum;
using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Services
{    public interface ICartService : IDomainService
    {
        Task<CartItem?> AddOrderTagsToCart(long orderId, MenuItem menuItem, MealItOptions mealItOptions, int quantity, IReadOnlyCollection<TagWithQuantity> orderTags);

        Task<Cart> GetCartDetails(long orderId);

        Task<bool> ClearCart(long orderId);
        Task<bool> AddQuantity(long orderId);
    }
}
