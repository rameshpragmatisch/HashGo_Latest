
namespace HashGo.Core.Models.Ticket
{
    public class FlyPayDetails
    {
        public int TicketId { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentTag { get; set; }
        public PaymentFrame PaymentFrame { get; set; }
        public string Log { get; set; }
        public string AppliedTerminal { get; set; }

    }

    public class FlyPaymentSalesResponse
    {
        public int PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public decimal TotalAmount { get; set; }
    }
}