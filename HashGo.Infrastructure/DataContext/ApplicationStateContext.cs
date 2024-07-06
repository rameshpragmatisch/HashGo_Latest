using HashGo.Core.Db;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Models;
using Newtonsoft.Json.Linq;

namespace HashGo.Infrastructure.DataContext
{
    public static class ApplicationStateContext
    {
        public static List<TenantConnect> TenantConnectItems = new List<TenantConnect>();
        public static string OrderQueue = string.Empty;
        public static int DepartmentId = 0;
        public static CustomerDetails? CustomerDetailsObj = null;
        //public static string? CustomerDate = null;
        public static DateTime CustomerDate = DateTime.Now;
        public static bool IsMorningTime = false;
        public static bool IsEveningTime = false;
        public static TenantConnect ConnectItem = null;
        public static readonly int IdleTimeOutInSecs = 30; 

        static ApplicationStateContext()
        {
            LoadSettings();
        }

        public static void ClearData()
        {
            CustomerDate = DateTime.Now;
            DepartmentId = 0;
            CustomerDetailsObj = null;
            IsMorningTime = false;
            IsEveningTime = false;
        }

        public static void LoadSettings()
        {
            HashGoAppSettings.LoadSettings();
            ConnectItem = new TenantConnect()
            {
                Url = HashGoAppSettings.Url,
                User = HashGoAppSettings.User,
                Password = HashGoAppSettings.Password,
                TenantName = HashGoAppSettings.Tenant,
                DeviceId = HashGoAppSettings.DeviceId,
                LocationId = HashGoAppSettings.LocationId,
                TenantId = HashGoAppSettings.TenantId,
                SortOrder = string.IsNullOrEmpty(HashGoAppSettings.SortOrder) ? 0 : Convert.ToInt32(HashGoAppSettings.SortOrder),
                 PaymentScreenVisibleDelay = HashGoAppSettings.PaymentScreenVisibleDelay
            };
        }
    }
}
