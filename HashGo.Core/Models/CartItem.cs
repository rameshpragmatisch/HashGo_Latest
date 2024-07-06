using HashGo.Core.Enum;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HashGo.Core.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        public CartItem()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
            TagWithQuantities = Array.Empty<TagWithQuantity>();
            this.MealItOption = MealItOptions.None;
        }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LatestUpdatedOn { get; set; }

        public int MenuItemId { get; set; }

        public long ResturantId { get; set; }

        public string ResturantUniqueId { get; set; }

        public string WorkFlowName { get; set; }

        public string RestaurantBanner { get; set; }

        public string BrandColor { get; set; }

        public string ComboType { get; set; }

        public string EstimatedWaitingTime
        {
            get { return "Estimated Waiting Time " + WaitingTime + " Minutes"; }
        }

        public string WaitingTime { get; set; }

        private int _Quantity { get; set; }

        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                Price = this.CalculateItemPrice() * this.Quantity;
                OnPropertyChanged();
            }
        }

        private decimal _Price;
        public decimal Price 
        {
            get
            {
                return _Price = this.CalculateItemPrice() * this.Quantity;
            }
            set
            {
                _Price = value;
                OnPropertyChanged();
            }
        }

        private bool _HasOrderTags;
        public bool HasOrderTags
        {
            get
            {
                return _HasOrderTags;
            }
            set
            {
                _HasOrderTags = value;
                OnPropertyChanged();
            }
        }

        public virtual MenuItem MenuItem { get; set; }

        public MealItOptions MealItOption { get; set; } 

        public TagWithQuantity[] TagWithQuantities { get; set; }


        private decimal CalculateItemPrice()
        {
            var cartItemBasePrice = 0.0M;

            if(this.MenuItem != null && this.MenuItem.NormalPortion != null)
                cartItemBasePrice = (decimal)this.MenuItem.NormalPortion?.Price;

            var cartItemTagPrice = 0M;


            foreach (var tagItem in this.TagWithQuantities)
            {
                cartItemTagPrice += tagItem.TotalPrice;
            }

            cartItemTagPrice = cartItemBasePrice + cartItemTagPrice;

            return cartItemTagPrice;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
