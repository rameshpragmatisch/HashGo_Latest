namespace HashGo.Infrastructure.Common;

public static class ConnectApiRouterNames
{
    public const string LOGIN = "/api/Account/Authenticate";
    public const string DEVICE_DETAIL = "/api/services/app/dineGoDevice/ApiGetDeviceDetail";
    public const string ITEMS_LOCATION = "/api/services/app/menuItem/ApiItemsForLocation";
    public const string SCREEN_MENU = "/api/services/app/screenMenu/ApiScreenMenu";
    public const string ORDER_TAG = "/api/services/app/orderTagGroup/ApiGetTags";
    public const string ESTIMATE_WAITINGTIME = "/setting/retrieve/DineGoWaitingTime";
}

public static class PlanApiRouterNames
{
    public const string DINE_PLAN_PUSH_FLYTICKET = "/flyticket/push";
    public const string DINE_PLAN_PUSH_JUST_FLYTICKET = "/flyticket/justpush";
    public const string DINE_PLAN_SOLD_OUTS = "flyticket/getsoldouts";
    public const string DINE_PLAN_PUSH_FLYPAYMENT = "/payment/pay";
    public const string DINE_PLAN_PRINT_CONTENT = "/flyticket/billcontent/{0}";
    public const string DINEPLAN_SUMMARY_SALE = "/payment/sales/{0}";
}

public static class DineGatewayManager
{
    public const string DINE_GATEWAY_URL = "https://dgateway.xyz/v1/ticket/create";
}