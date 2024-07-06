using HashGo.Core.Db;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.DataContext;
using HashGo.Domain.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.Repository
{
    public class QueueSettingRepository : RepositoryBase<HashGoCacheContext, QueueSettings, int>, IQueueSettingRepository
    {
    }
}
