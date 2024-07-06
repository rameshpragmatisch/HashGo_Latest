using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Db;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.Services.Store
{
    public class QueueSettingStoreService : BaseStoreService<QueueSettings>, IQueueSettingStoreService
    {
        public QueueSettingStoreService(ILoggingService loggingService, IQueueSettingRepository queueSettingRepository)
            : base(loggingService, queueSettingRepository)
        {
        }
    }
}
