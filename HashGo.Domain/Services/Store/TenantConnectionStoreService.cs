using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Db;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.Services.Base;

namespace HashGo.Domain.Services.Store;

public class TenantConnectionStoreService : BaseStoreService<TenantConnect>, ITenantConnectStoreService
{
    public TenantConnectionStoreService(ILoggingService loggingService, ITenantConnectRepository tenantConnectRepository)
        : base(loggingService, tenantConnectRepository)
    {
    }
}