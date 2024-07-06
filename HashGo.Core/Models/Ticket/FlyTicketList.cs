#region using

using System.Collections.Generic;

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyTicketIdList
    {
        public FlyTicketIdList()
        {
            TicketIds = new List<int>();
        }

        public List<int> TicketIds { get; set; }
    }
}