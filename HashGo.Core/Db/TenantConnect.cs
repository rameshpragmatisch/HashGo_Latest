using HashGo.Core.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HashGo.Core.Db;
public class TenantConnect : BaseEntity
{
    

    public string Url { get; set; }
    public string TenantName { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string TenantId { get; set; }
    public string LocationId { get; set; }
    public string DeviceId { get; set; }
    public int SortOrder { get; set; }
    public int WaitingTime { get; set; }
    public DateTime LastUpdatedTime { get; set; } = DateTime.Now;
    private List<string> ConnectKeys => [Url.Replace(HashGoConst.httpString, string.Empty), TenantName, TenantId, LocationId, DeviceId];

    private string _TenantUniqueKey;

    [NotMapped]
    public string TenantUniqueKey
    {
        get
        {
            _TenantUniqueKey = string.Join("|", ConnectKeys);
            return _TenantUniqueKey;
        }
        set
        {
            _TenantUniqueKey = value;
        }
    }

    public string PaymentScreenVisibleDelay { get; set; }
    public string NETSPort { get; set; }
}
