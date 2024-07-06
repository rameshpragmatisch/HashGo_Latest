#region using

#endregion

using System.Collections.Generic;

namespace DinePlan.Common.Model.Point
{
    public class FlySoldOut
    {
        public int MenuItemId { get; set; }
        public string PortionName { get; set; }
        public string MenuItemName { get; set; }
        public decimal Quantity { get; set; }
        public int PortionId { get; set; }
        public int PortionSyncId { get; set; }
    }

    public class FlySoldOutTest
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
    }

    public class FlySoldOutList
    {
        public FlySoldOutList()
        {
            SoldOut = new List<FlySoldOut>();
        }

        public List<FlySoldOut> SoldOut { get; set; }
    }
}