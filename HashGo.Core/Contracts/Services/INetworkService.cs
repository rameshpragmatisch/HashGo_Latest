using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Services
{
    public interface INetworkService : IDomainService
    {
        bool ProcessFlyOrder(string requestBody, RestaurantBrand restaurantBrand);
        string GetQueueNumber();
    }
    
}
