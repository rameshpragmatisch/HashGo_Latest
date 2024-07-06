#region using

#endregion

using System.Collections.Generic;

namespace DinePlan.Common.Model.Point
{
    public class FlySoldOutOrderTag
    {
       
        public int OrderTagId { get; set; }
        public string OrderTagName { get; set; }
        public int OrderTagSyncId { get; set; }
        public decimal Quantity { get; set; }
    }

  

    public class FlySoldOutOrderTagList
    {
        public FlySoldOutOrderTagList()
        {
            SoldOut = new List<FlySoldOutOrderTag>();
        }

        public List<FlySoldOutOrderTag> SoldOut { get; set; }
    }
}