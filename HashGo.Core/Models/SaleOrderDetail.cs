namespace HashGo.Core.Models
{
    public class SaleOrderDetail
    {
        public int ProductId { get; set; }

        public int UnitId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Qty { get; set; }

        public decimal Price { get; set; }

    }
}
