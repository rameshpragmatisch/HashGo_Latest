#region using

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyChangeEntity
    {
        public int TicketId { get; set; }
        public int EntityId { get; set; }
    }

    public class FlyMemberEntity
    {
        public string customerPhone { get; set; }
        public string customerName { get; set; }
        public string pax { get; set; }
        public string tables { get; set; }
        public string merchantId { get; set; }
        public string merchangeKey { get; set; }
    }
}