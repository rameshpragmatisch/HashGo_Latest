using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Db;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.Services.Base;

namespace HashGo.Domain.Services.Store;

public class ProductDetailStoreService :  BaseStoreService<ProductDetail>, IProductDetailStoreService
{
    public ProductDetailStoreService(ILoggingService loggingService, IProductDetailRepository tenantConnectRepository)
        : base(loggingService, tenantConnectRepository)
    {
    }

}