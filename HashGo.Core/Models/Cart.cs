using System.Collections.ObjectModel;

namespace HashGo.Core.Models
{
    public class Cart
    {

        public Cart()
        {
            Items = new ObservableCollection<CartItem>();
            CouponCode = string.Empty;
        }

        public ObservableCollection<CartItem> Items { get; set; }

        public string CouponCode { get; set; }

        public decimal SubTotal
        {
            get
            {
                return this.GetSubTotalAmount();
            }
        }

        public decimal Discount { get; set; }

        public decimal SubTotalWithDiscount
        {
            get
            {
                return SubTotal - Discount;
            }
        }

        private decimal GetSubTotalAmount()
        {
            var subTotal = 0.0M;

            foreach (var cartItem in this.Items)
            {
                subTotal += cartItem.Price;
            }

            return subTotal;
        }



    //public bool IsValid
    //{
    //    get
    //    {
    //        foreach (var item in Items)
    //        {
    //            if (!item.IsProductAvailabeToOrder)
    //            {
    //                return false;
    //            }

    //            if (item.ProductStockTrackingIsEnabled && item.ProductStockQuantity < item.Quantity)
    //            {
    //                return false;
    //            }
    //        }

    //        return true;
    //    }
    //}
}
}
