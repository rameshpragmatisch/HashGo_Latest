using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Constants;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Domain.ViewModels
{
    public partial class CartViewViewModel : BaseResturantViewModel
    {
        private const decimal TaxPercentage = 0.17M;
        private ICartService _cartService;

        [ObservableProperty]
        string alias;

        [ObservableProperty]
        string contactNumber;

        [ObservableProperty]
        string voucherCode;

        [ObservableProperty]
        decimal subtotal;

        [ObservableProperty]
        decimal promotion;

        [ObservableProperty]
        decimal tax;

        [ObservableProperty]
        decimal total;

        [ObservableProperty]
        Cart cart;

        public CartViewViewModel(ILoggingService loggingService,
                                 IRestaurantBrandService brandService,
                                 INavigationService navigationService,
                                 IOrderService orderService,
                                 ICartService cartService)
            : base(loggingService, brandService, navigationService, orderService)
        {
            _cartService = cartService;
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        private void SetSubTotalAmount(decimal subTotalAmount)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SetSubTotalAmount)}() Started.");

            this.Subtotal = subTotalAmount;

            if (this.Subtotal > 0)
            {
                this.Tax = this.Subtotal * TaxPercentage;
                this.Promotion = 2.9M;
                this.Total = this.Subtotal - this.Promotion + this.Tax;
                this.Total = RoundOffTotal(this.Total);
            }
            else
            {
                this.Tax = 0;
                this.Promotion = 0;
                this.Total = 0;
            }

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SetSubTotalAmount)}() Completed.");
        }

        private decimal RoundOffTotal(decimal total)
        {
            if (total < 0)
                total = 0;

            return total;
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(LoadDataAsync)}() Started.");

            await CalculateCartPrice();

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private async Task CalculateCartPrice()
        {
            this.Cart = await this._cartService.GetCartDetails(this.OrderId);

            var subTotalAmount = this.Cart.SubTotal;

            this.SetSubTotalAmount(subTotalAmount);
        }


        private bool CanProceedToCheckout() { return !this.IsLoading; }


        [RelayCommand(CanExecute = nameof(CanProceedToCheckout))]
        private async Task ProceedToCheckout(MenuItem menuItem)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToCheckout)}() Started.");

            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                { nameof(this.Cart), this.Cart },
            };

            await this.NavigateToPage(Pages.PaymentMethod, parameters);

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToCheckout)}() Completed.");
        }

        private bool CanProceedToStartOver() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanProceedToCheckout))]
        private async Task ProceedToStartOver(MenuItem menuItem)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToStartOver)}() Started.");

            await this.ClearCartCommand.ExecuteAsync(null);

            await this.NavigateToBrandSelectionCommand.ExecuteAsync(null);

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToStartOver)}() Completed.");
        }

        [RelayCommand]
        private async Task ClearCart()
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ClearCart)}() Started.");

            var isCartCleared = await this._cartService.ClearCart(this.OrderId);
            if (isCartCleared)
            {
                this.Cart = null;
                await this.LoadDataAsync();
            }

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ClearCart)}() Completed.");

        }

        [RelayCommand]
        private async Task AddQuantityInCart(MenuItem menuItem)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(AddQuantityInCart)}() Started.");

            foreach (var item in Cart.Items)
            {
                if(item.MenuItem != null && item.MenuItem.Id == menuItem.Id)
                {
                    item.Quantity += 1;

                    var subTotalAmount = this.Cart.SubTotal;
                    this.SetSubTotalAmount(subTotalAmount);
                    break;
                }
            }



            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(AddQuantityInCart)}() Completed.");
        }

        [RelayCommand]
        private async Task SubtractQuantityInCart(MenuItem menuItem)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SubtractQuantityInCart)}() Started.");

            foreach (var item in Cart.Items)
            {
                if (item.MenuItem != null && item.MenuItem.Id == menuItem.Id)
                {
                    if(item.Quantity  > 1)
                        item.Quantity -= 1;

                    var subTotalAmount = this.Cart.SubTotal;
                    this.SetSubTotalAmount(subTotalAmount);
                    break;
                }
            }

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SubtractQuantityInCart)}() Completed.");
        }

        private bool CanEditOrder(CartItem cartItem) { return !this.IsLoading && cartItem != null; }

        [RelayCommand(CanExecute = nameof(CanEditOrder))]
        private async Task EditOrder(CartItem cartItem)
        {
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SubtractQuantityInCart)}() Started.");

            if (cartItem != null)
            {
                await NavigateToMenuDetails(cartItem);
            }

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(SubtractQuantityInCart)}() Completed.");
        }

        private async Task NavigateToMenuDetails(CartItem cartItem)
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Started.");

            var workFlowName = MealItOptionHelper.GetWorkFlowByComboName(cartItem.WorkFlowName);
            var mealItOptions = MealItOptionHelper.GetComboOptionByName(cartItem.WorkFlowName);
            var lstSeelctedTags = cartItem.TagWithQuantities;
            var restaurantBrands = await this.ResturantBrandService.ReadAllBrands();
            var requiredBrand = restaurantBrands.FirstOrDefault(x => x.TenantUniqueKey == cartItem.ResturantUniqueId);

            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { "SelectedMenuItem", cartItem.MenuItem },
                { "SelectedMealItOption", mealItOptions },
                { "EditOrderTagsWithQuantity", lstSeelctedTags },
                { "SelectedRestaurant", requiredBrand },
                { "IsEditOrder", true },
            };

            await NavigationService.NavigateToAsync(workFlowName, parameters);

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Completed.");
        }
    }
}
