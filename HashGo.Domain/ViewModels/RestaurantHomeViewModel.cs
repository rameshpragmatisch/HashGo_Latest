using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Models;
using System.Collections.ObjectModel;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HashGo.Domain.ViewModels
{
    public partial class RestaurantHomeViewModel : BaseResturantViewModel
    {
        IViewService viewService;

        [ObservableProperty]
        IEnumerable<RestaurantBrand> restaurants = Array.Empty<RestaurantBrand>();

        [ObservableProperty]
        IEnumerable<IView> mainViews;

        [ObservableProperty]
        IView selectedView;

        ICartService cartService;

        //Cart cart;

        [ObservableProperty]
        bool isCartEmpty = false;

        [ObservableProperty]
        int cartItemsCount = 0;

        [ObservableProperty]
        decimal totalPrice = 0M;

        private ObservableCollection<IView> views;
        private string selectedRestaurantName;


        TenantConnect[] connectItems = Array.Empty<TenantConnect>();

        public RestaurantHomeViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                       INavigationService navigationService,
                                       IOrderService orderService,
                                       IViewService viewService,
                                       ICartService cartService)
            : base(loggingService, brandService, navigationService, orderService)
        {
            this.viewService = viewService;
            this.cartService = cartService;
        }
        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            views = new ObservableCollection<IView>();

            await this.LoadDataAsync();

            this.MainViews = views;

            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        //private IView selectedView;

        //public IView SelectedView
        //{
        //    get { return selectedView; }
        //    set
        //    {
        //        selectedView = value;
        //        OnPropertyChanged(nameof(SelectedView));
        //    }
        //}
        public static RestaurantBrand LastSelectedRestaurantBrand;
        private RestaurantBrand selectedRestaurant;

        public RestaurantBrand SelectedRestaurant
        {
            get { return selectedRestaurant; }
            set
            {
                selectedRestaurant = value;
                if (LastSelectedRestaurantBrand == null)
                    views?.Clear();
                    LastSelectedRestaurantBrand = selectedRestaurant;
                OnPropertyChanged(nameof(SelectedRestaurant));
            }
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(LoadDataAsync)}() Started.");

            var selectedRestauratId = this.SelectedRestaurant?.Id;
            selectedRestaurantName = this.SelectedRestaurant?.Name;

            this.Restaurants = new ObservableCollection<RestaurantBrand>(await this.BrandService.ReadAllBrands());

            if (selectedRestaurantName != null)
            {
                this.SelectedRestaurant = this.Restaurants.FirstOrDefault(x => x.Name == selectedRestaurantName);
            }
            if (views != null)
                Refresh();



            var cart = await this.cartService.GetCartDetails(OrderId);
            SetTagGroupDisplay(ref cart);

            if (cart?.Items?.Any() == true)
            {
                this.IsCartEmpty = false;
                this.CartItemsCount = cart.Items.Count;
                this.TotalPrice = cart.SubTotal;

            }


            this.IsCartEmpty = cart != null && !cart.Items.Any();
            this.CartItemsCount = cart.Items.Count;
            this.TotalPrice = Convert.ToDecimal(cart.SubTotal);



            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private void SetTagGroupDisplay(ref Cart cart)
        {
            if (cart != null && cart.Items != null && cart.Items.Any())
            {
                foreach (var cartItem in cart.Items)
                {
                    if (cartItem.TagWithQuantities != null && cartItem.TagWithQuantities.Count() > 1)
                    {
                        var lastGroupItem = string.Empty;

                        foreach (var item in cartItem.TagWithQuantities)
                        {
                            if (lastGroupItem == item.GroupDisplayName)
                            {
                                item.GroupDisplayName = string.Empty;
                                item.TagGroupDisplayMerged = true;
                            }
                            else
                                lastGroupItem = item.GroupDisplayName;
                        }
                    }
                }
            }
        }
        private void Refresh()
        {
            foreach (var restaurant in Restaurants)
            {
                var view = GetView(Core.Enum.Views.ProductMenu.ToString(), this.viewService, this.Logger);
                if (view != null)
                {

                    if (this.MainViews == null || Restaurants.Count() > views.Count)
                    {
                        views.Add(view);

                        if (view is IHasDataContext hasDataContext &&
                            hasDataContext.DataContext is BrandMenuViewModel menuViewModel)
                        {
                            menuViewModel.SelectedRestaurant = restaurant;
                            menuViewModel.MainView = view;
                            menuViewModel.OrderId = this.OrderId;

                            if (selectedRestaurantName != null && restaurant.Name == selectedRestaurantName)
                            {
                                this.SelectedView = view;
                            }
                        }
                    }
                }
            }

        }


        private bool CanViewCart() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanViewCart))]
        private async Task ViewCart()
        {
            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(ViewCart)}() Started.");


            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
            };

            await NavigationService.NavigateToAsync(Pages.CartView.ToString(), parameters);

            this.Logger.Trace($"{nameof(RestaurantHomeViewModel)} : {nameof(ViewCart)}() Completed.");

        }


        private bool CanProceedToStartOver() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanProceedToStartOver))]
        private async Task ProceedToStartOver(MenuItem menuItem)
        {
            RestaurantHomeViewModel.LastSelectedRestaurantBrand = null;
            BrandMenuViewModel.LastSelectedItemIndex = 0;
            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToStartOver)}() Started.");

            await this.cartService.ClearCart(this.OrderId);

            await this.NavigateToBrandSelectionCommand.ExecuteAsync(null);

            this.Logger.Trace($"{nameof(CartViewViewModel)} : {nameof(ProceedToStartOver)}() Completed.");
        }
    }
}
