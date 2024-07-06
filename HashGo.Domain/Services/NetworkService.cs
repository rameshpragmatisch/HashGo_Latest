using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Db;
using HashGo.Core.Models;
using HashGo.Domain.DataContext;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.HttpHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HashGo.Domain.Services
{
    public class NetworkService : INetworkService
    {
        private readonly ITenantConnectStoreService tenantConnectStoreService;
        private readonly ILoggingService logger;
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        private readonly IQueueSettingStoreService queueSettingDetailService;


        public NetworkService(ILoggingService logger, ITenantConnectStoreService tenantConnectStoreService, IQueueSettingStoreService queueSettingService)
        {
            this.logger = logger;
            this.tenantConnectStoreService = tenantConnectStoreService;
            this.queueSettingDetailService = queueSettingService;
        }

        public void Login(HttpHelper Instance, string _tenancyName,string _usernameOrEmailAddress, string _password)
        {
            try
            {
                string? responeString = Instance.Post(
                    JsonConvert.SerializeObject(new
                    {
                        tenancyName = _tenancyName,
                        usernameOrEmailAddress = _usernameOrEmailAddress,
                        password = _password
                    }),
                    ConnectApiRouterNames.LOGIN);
                BaseResponse<string>? result = JsonConvert.DeserializeObject<BaseResponse<string>>(responeString);
                if (result != null && result.Success)
                {
                    HttpHelper.GetInstance().SetToken(result.Result);
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }
        }

        public string GetQueueNumber()
        {
            string queueNumber = string.Empty;

            var qSDataFromDB = this.queueSettingDetailService.ReadAll();

            if (qSDataFromDB != null && qSDataFromDB.Any())
            {
                var qSData = qSDataFromDB.First();

                if (qSData.CurrentNumber >= qSData.EndNumber)
                    qSData.CurrentNumber = qSData.StartNumber;
                else
                {
                    if (qSData.CurrentNumber <= 0)
                        qSData.CurrentNumber = qSData.StartNumber;
                    else
                        qSData.CurrentNumber++;
                }

                queueNumber = qSData.Prefix + qSData.CurrentNumber + qSData.Suffix;

                queueSettingDetailService.AddOrUpdateSync(qSData);
            }

            return queueNumber;
        }

        public bool ProcessFlyOrder(string requestBody, RestaurantBrand restaurantBrand)
        {
            var tenantItem = ApplicationStateContext.TenantConnectItems.FirstOrDefault(x => x.TenantUniqueKey == restaurantBrand.TenantUniqueKey);

            if (tenantItem != null)
            {
                var httpInstance = HttpHelper.GetInstance(tenantItem.Url);
                httpInstance.SetToken(restaurantBrand.DineGatewayToken);
                var responeString = httpInstance.Post(requestBody, DineGatewayManager.DINE_GATEWAY_URL);

                if (responeString != null)
                {

                }
            }

            return true;
        }
    }
}
