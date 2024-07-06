using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class SoldOutItem
    {
        public int MenuItemId { get; set; }
        public string PortionName { get; set; }
        public string MenuItemName { get; set; }
        public double Quantity { get; set; }
        public int PortionId { get; set; }
        public int PortionSyncId { get; set; }
    }

    public class SoldOutResponse
    {
        public List<SoldOutItem> SoldOut { get; set; }
    }

    public class SoldOut
    {
        public bool IsError { get; set; }
        public string Error { get; set; }
        public SoldOutResponse Response { get; set; }
    }
}
