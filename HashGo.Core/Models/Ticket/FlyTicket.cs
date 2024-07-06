#region using

using System.ComponentModel;

#endregion

namespace HashGo.Core.Models.Ticket
{
    public class FlyTicket
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastOrderDate { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public bool IsLocked { get; set; }
        public bool IsClosed { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public int RefId { get; set; }
        public bool IsPaid { get; set; }
        public string TicketTags { get; set; }
        public string UserName { get; set; }
        public string Pax { get; set; }
        public string TicketNumber { get; set; }
        public bool TaxIncluded { get; set; }
        public bool IsDineCall { get; set; }
        public string QueueNumber { get; set; }
        public int? AlternateId { get; set; }
        public bool SkipSoldOut { get; set; }

        public FlyEntityStatus FlyEntityStatus { get; set; }
        public FlyTicketStatus FlyTicketStatus { get; set; }
        public List<FlyOrder> Orders { get; set; }
        public List<FlyTransaction> Transactions { get; set; }
        public List<FlyTicketEntity> Entities { get; set; }
        public List<FlyCalculation> Calculations { get; set; }
        public List<FlyPayDetails> Payments { get; set; }
    }
    public class FlyOrder
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
        public int MenuItemType { get; set; }
        public int OrderRef { get; set; }
        public string FlyOrderStatus { get; set; }
        public List<FlyOrderStatus> OrderStatus { get; set; }
        public List<FlyOrderTagItems> OrderTagItems { get; set; }
        public string UserName { get; set; }
        public int RefId { get; set; }
        public string CustomStatus { get; set; }
        public int MenuItemSyncId { get; set; }

        public FlyOrder()
        {
            FlyOrderStatus = "New";
            OrderStatus = new List<FlyOrderStatus>
            {
                Ticket.FlyOrderStatus.New
            };
        }
    }

    public class FlyCalculation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class FlyTicketEntity
    {
        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityName { get; set; }
        public string EntityTypeName { get; set; }
        public string CustomData { get; set; }
    }

    public class FlyCustomField
    {
        public string Name { get; set; }
        public int FieldType { get; set; }
        public string EditingFormat { get; set; }
        public string ValueSource { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class FlyTransaction
    {
        public TrasactionType TrasactionType { get; set; }
        public decimal Amount { get; set; }
    }

    public enum TrasactionType
    {
        Sales,
        ServiceCharge,
        Tax,
        Discount,
        Round
    }

    public enum FlyEntityStatus
    {
        [Description("New Orders")] NewOrders,

        [Description("Bill Requested")] BillRequested,

        [Description("Available")] Available,

        [Description("TableNotReset")] TableNotReset

    }

    public enum FlyTicketStatus
    {
        [Description("New Orders")] NewOrders,

        [Description("Locked")] Locked,

        [Description("Unpaid")] Unpaid,

        [Description("Paid")] Paid
    }

    public enum FlyOrderStatus
    {
        [Description("New")] New,
        [Description("Submitted")] Submitted,
        [Description("Serve Now")] ServeNow,
        [Description("Serve Later")] ServeLater,
        [Description("Void")] Void,
        [Description("Gift")] Gift,
        [Description("Urgent")] Urgent,
        [Description("Served")] Served,
        [Description("Reprint")] Reprint

    }

    public class FlyOrderTagItems
    {
        public int OrderTagGroupId { get; set; }
        public int OrderTagId { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}