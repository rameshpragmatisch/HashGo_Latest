using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Db
{
    public class ProductDetail : BaseEntity
    {
        public string TenantUniqueKey { get; set; }
        public string DeviceDetails { get; set; }
        public string ScreenMenuDetails { get; set; }
        public string OrderTagDetails { get; set; }
        public string LocationItemDetails { get; set; }
    }

    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
