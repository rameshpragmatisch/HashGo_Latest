using HashGo.Core.Db;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.DataContext;
using HashGo.Domain.Repository.Base;

namespace HashGo.Domain.Repository
{
    public class TenantConnectRepository : RepositoryBase<HashGoCacheContext, TenantConnect, int>, ITenantConnectRepository
    {
    }
}
