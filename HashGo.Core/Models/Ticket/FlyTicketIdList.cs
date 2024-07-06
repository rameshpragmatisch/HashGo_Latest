#region using

using System.Collections.Generic;
using HashGo.Core.Models.Ticket;

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyTicketList
    {
        public FlyTicketList()
        {
            Tickets = new List<FlyTicket>();
        }

        public List<FlyTicket> Tickets { get; set; }
    }
}