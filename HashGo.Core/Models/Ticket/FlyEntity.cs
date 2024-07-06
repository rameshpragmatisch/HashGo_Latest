#region using

using System.Collections.Generic;

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyEntity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int EntityScreenId { get; set; }
        public string EntityScreenName { get; set; }
        public int SortOrder { get; set; }
        public IEnumerable<FlyEntityTicketData> TicketData { get; set; }
        public int ButtonHeight { get; set; }
        public bool Occupied { get; set; }
    }

    public class FlyEntityTicketData
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}