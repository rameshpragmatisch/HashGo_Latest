using HashGo.Core.Contracts.Repository;
using HashGo.Core.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.Contracts.Repository
{
    public interface IQueueSettingRepository : IRepositoryBase<QueueSettings, int>
    {
    }
}
