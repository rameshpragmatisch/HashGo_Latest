using HashGo.Core.Enum;
using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Services
{
    public interface IWorkFlowService : IDomainService
    {
        Task<WorkFlow> ReadWorkFlowAsync(long restaurantId, long menuItemId, DiningOption orderDiningOption, MealItOptions mealItOption);
    }
}
