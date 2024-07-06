using HashGo.Core.Contracts.Services;
using HashGo.Core.Models;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.HttpHelper;
using Newtonsoft.Json;

namespace HashGo.Domain.Services;

public class ProductService : IProductService
{
    private readonly ILoggingService logger;

    public ProductService(ILoggingService logger)
    {
        this.logger = logger;
    }

    public string GetDeviceDetail(string baseUrl, string tenantId, string deviceId)
    {
        try
        {
            var myInstance = HttpHelper.GetInstance(baseUrl);

            var responeString = myInstance.Post(
                JsonConvert.SerializeObject(new
                {
                    Id = int.Parse(deviceId),
                    tenantId = int.Parse(tenantId),
                }),
                ConnectApiRouterNames.DEVICE_DETAIL);
            return responeString;
        }
        catch (Exception ex)
        {
            logger.TraceException(ex);
        }

        return "";
    }

    public string GetItemsForLocation(string baseUrl, string tenantId, string locationId)
    {
        try
        {
            var responseString = HttpHelper.GetInstance(baseUrl).Post(
                JsonConvert.SerializeObject(new
                {
                    tenantId = int.Parse(tenantId),
                    locationId = int.Parse(locationId)
                }),
                ConnectApiRouterNames.ITEMS_LOCATION);
            return responseString;
        }
        catch (Exception ex)
        {
            logger.TraceException(ex);
        }

        return "";
    }

    public string GetScreenMenu(string baseUrl, string tenantId, string locationId)
    {
        try
        {
            var responseString = HttpHelper.GetInstance(baseUrl).Post(
                JsonConvert.SerializeObject(new
                {
                    tenantId = int.Parse(tenantId),
                    locationId = int.Parse(locationId)
                }),
                ConnectApiRouterNames.SCREEN_MENU);
            return responseString;
        }
        catch (Exception ex)
        {
            logger.TraceException(ex);
        }

        return "";
    }

    public string GetOrderTags(string baseUrl, string tenantId, string locationId)
    {
        try
        {
            var responseString = HttpHelper.GetInstance(baseUrl).Post(JsonConvert.SerializeObject(new
                {
                    tenantId = int.Parse(tenantId),
                    locationId = int.Parse(locationId)
                }),
                ConnectApiRouterNames.ORDER_TAG);
            return responseString;
        }
        catch (Exception ex)
        {
            logger.TraceException(ex);
        }

        return "";
    }
}