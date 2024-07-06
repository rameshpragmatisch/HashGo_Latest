using HashGo.Core.Models.Ticket;

namespace DinePlan.Common.Model.Point
{
    public class FlyUtility
    {
        public static FlyTicketStatus GetFlyTicketStatus(string state)
        {
            if (state.Equals("New Orders")) return FlyTicketStatus.NewOrders;
            if (state.Equals("Unpaid")) return FlyTicketStatus.Unpaid;
            if (state.Equals("Locked")) return FlyTicketStatus.Locked;
            return FlyTicketStatus.NewOrders;
        }

        public static FlyOrderStatus GetFlyOrderStatus(string state)
        {
            if (state.Equals("New")) return FlyOrderStatus.New;
            if (state.Equals("Submitted")) return FlyOrderStatus.Submitted;
            if (state.Equals("Serve Now")) return FlyOrderStatus.ServeNow;
            if (state.Equals("Serve Later")) return FlyOrderStatus.ServeLater;
            if (state.Equals("Void")) return FlyOrderStatus.Void;
            if (state.Equals("Gift")) return FlyOrderStatus.Gift;
            if (state.Equals("Urgent")) return FlyOrderStatus.Urgent;
            if (state.Equals("Served")) return FlyOrderStatus.Served;
            if (state.Equals("Reprint")) return FlyOrderStatus.Reprint;

            return FlyOrderStatus.New;
        }
    }
}