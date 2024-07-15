using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.HttpHelper;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Category = HashGo.Core.Models.BestTech.Category;

namespace HashGo.Domain.Services
{
    public class RetailConnectService : IRetailConnectService
    {
        private readonly ITenantConnectStoreService tenantConnectStoreService;
        private readonly ILoggingService logger;
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        private readonly IQueueSettingStoreService queueSettingDetailService;


        public RetailConnectService(ILoggingService logger, ITenantConnectStoreService tenantConnectStoreService, IQueueSettingStoreService queueSettingService)
        {
            this.logger = logger;
            this.tenantConnectStoreService = tenantConnectStoreService;
            this.queueSettingDetailService = queueSettingService;
            HashGoAppSettings.LoadSettings();

            if(!string.IsNullOrEmpty(HashGoAppSettings.Url) &&
                !string.IsNullOrEmpty(HashGoAppSettings.User) &&
                !string.IsNullOrEmpty(HashGoAppSettings.Password) &&
                !string.IsNullOrEmpty(HashGoAppSettings.Tenant))
            {
                this.Login(HttpHelper.GetInstance(), HashGoAppSettings.Tenant, HashGoAppSettings.User, HashGoAppSettings.Password);
            }
            else
            {
                logger.Info("Please provide valid settings inorder to connect to the service.");
            }
        }

        public void Login(HttpHelper Instance, string _tenancyName, string _usernameOrEmailAddress, string _password)
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
                    ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.LOGIN);
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


        public async Task<IReadOnlyCollection<Department>> GetAllDepartments()
        {
            // Todo make sure login is done and token is set
            List<Department> departments = new List<Department>();
            try
            {
                var client = HttpHelper.GetInstance();
                if (client == null) return departments;

                string? responeString = client.Post(
                        JsonConvert.SerializeObject(new
                        {
                            tenantId = ApplicationStateContext.ConnectItem.TenantId,
                            LastSyncDateTime = "",
                        }),
                        //AppConfigKeys.RETAIL_CONNECT_URL+ RetailConnectApiRouterNames.GET_ALL_DEPARTMENT);
                        ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_ALL_DEPARTMENT);

                DepartmentRoot result = JsonConvert.DeserializeObject<DepartmentRoot>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return (departments = result.result.items);
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return departments;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoryiesByDepartmentId(int deptId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           departmentId = deptId,
                       }),
                         ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_CATEGORIES_BY_DEPTID);

                Base<Category> result = JsonConvert.DeserializeObject<Base<Category>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<Category>();
        }

        public async Task<IReadOnlyCollection<SubCategory>> GetSubCategoriesByCategoryId(int catId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           Categoryid  = catId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_SUBCATEGORIES_BY_CATID);

                Base<SubCategory> result = JsonConvert.DeserializeObject<Base<SubCategory>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items.OrderBy(ee=>ee.id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<SubCategory>();
        }

        public async Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           categoryId = categoryId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_PRODUCTS_BY_CATEGORY_ID);

                Base<ServiceUnit> result = JsonConvert.DeserializeObject<Base<ServiceUnit>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<ServiceUnit>();
        }

        public async Task<TransactionDetails> CreateSalesOrderWithPayment(SalesOrderRequest saleOrder)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(saleOrder),

                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.POST_SALEORDER);

                SalesOrderResponse result = JsonConvert.DeserializeObject<SalesOrderResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return null;
        }

        public void TryLogin(string tenancyName, string usernameOrEmailAddress, string password)
        {
            this.Login(HttpHelper.GetInstance(), HashGoAppSettings.Tenant, HashGoAppSettings.User, HashGoAppSettings.Password);
        }

        public async Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryAndSubcategoryId(int categoryId, int subCtgryId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           categoryId = categoryId,
                           subCategoryId = subCtgryId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_PRODUCTS_BY_CTGRYANDSUBCATEGORY_ID);

                Base<ServiceUnit> result = JsonConvert.DeserializeObject<Base<ServiceUnit>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<ServiceUnit>();
        }

        public async Task<bool> CreateEnquiryRequest(EnquiriesRequestObject enquiriesRequest)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           enquiriesRequest.enquiry
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.CREATE_ENQUIRY);

                EnquiriesResponseObject result = JsonConvert.DeserializeObject<EnquiriesResponseObject>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return true;
                }
            }
            catch(Exception ex) { logger.TraceException(ex);}

            return  false;
        }

        public async Task<string> GetCompanyLogo(string locationId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           locationId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.CREATE_ENQUIRY);

                EnquiriesResponseObject result = JsonConvert.DeserializeObject<EnquiriesResponseObject>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return Convert.ToString(result.result);
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return CommonConstants.DEFAULTIMAGE;
        }
    }
}
