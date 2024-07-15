using static System.Net.WebRequestMethods;

namespace HashGo.Infrastructure.Common;

public static class RetailConnectApiRouterNames
{
    // TODO - /api/Account/Authenticate - to write a function that will return full URL
    public const string LOGIN = "api/Account/Authenticate";
    public const string GET_ALL_DEPARTMENT =  "/api/services/app/department/GetAll";
    public const string GET_CATEGORIES_BY_DEPTID = "/api/services/app/category/GetCategoriesByDepartment";
    public const string GET_SUBCATEGORIES_BY_CATID = "/api/services/app/subCategory/ApiGetSubCategoriesByCategory";
    public const string GET_PRODUCTS_BY_SUBCATEGORY_ID = "/api/services/app/product/GetApiProductsBySubCategory";
    public const string POST_SALEORDER = "/api/services/app/salesReceipt/ApiCreateSalesOrderWithPayment";
    public const string GET_PRODUCTS_BY_CTGRYANDSUBCATEGORY_ID = "/api/services/app/product/GetApiProductsByCategoryAndSubCategory";
    public const string GET_PRODUCTS_BY_CATEGORY_ID = "/api/services/app/product/GetApiProductsByCategoryId";
    public const string CREATE_ENQUIRY = "/api/services/app/Enquiry/CreateOrUpdateEnquiry";
    public const string GET_COMPANY_LOGO = "/api/services/app/company/GetCompanyImageByLocationId";
}
 
public static class AppConfigKeys
{
    public const string CONFIG_APPLCATIONURL = "ApplicationURL";
    public const string CONFIG_TENANT = "TenancyName";
    public const string CONFIG_USERNAME = "UserName";
    public const string CONFIG_PASSWORD = "Password";

    //public static readonly  string RETAIL_CONNECT_URL = System.Configuration.ConfigurationManager.AppSettings[CONFIG_APPLCATIONURL];
    //public static readonly string TENANT_NAME = System.Configuration.ConfigurationManager.AppSettings[CONFIG_TENANT];
    //public static readonly string USERNAME = System.Configuration.ConfigurationManager.AppSettings[CONFIG_USERNAME];
    //public static readonly string PASSWORD = System.Configuration.ConfigurationManager.AppSettings[CONFIG_PASSWORD];
}

public class CommonConstants
{
    public const string DEFAULTIMAGE = @"pack://application:,,,/Resources/Images/Home.png";
    public const string NOADDONIAMGE = @"pack://application:,,,/Resources/Images/NoAddOn.png";
}