using HashGo.Core.Models;

namespace HashGo.Core.Contracts.Services
{
    public interface IProductService : IDomainService
    {
        public string GetOrderTags(string baseUrl, string tenantId, string locationId);
        public string GetScreenMenu(string baseUrl, string tenantId, string locationId);
        public string GetItemsForLocation(string baseUrl, string tenantId, string locationId);
        public string GetDeviceDetail(string baseUrl,string tenantId, string deviceId);

    }
}