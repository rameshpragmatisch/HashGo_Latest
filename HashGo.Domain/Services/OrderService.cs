using HashGo.Core.Contracts.Services;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Core.Models.Ticket;
using HashGo.Domain.DataContext;
using HashGo.Infrastructure.DataContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.Services
{
    public class OrderService : IOrderService, ICartService
    {
        const int DelayTimeInMilliSec = 100;

        readonly Random randomNumberGenerator = new Random(DateTime.Now.Nanosecond);


        readonly Dictionary<long, Order> orderDictionary = new Dictionary<long, Order>();

        //private List<TagWithQuantity> _selectedOrderTags;

        //public List<TagWithQuantity> SelectedOrderTags
        //{
        //    get
        //    {
        //        return _selectedOrderTags;
        //    }
        //    set
        //    {
        //        _selectedOrderTags = value;
        //    }
        //}

        //private ICartService cartService;
        public OrderService()//ICartService cartService) 
        {
            //this.cartService = cartService;
        }

        public async Task SetOrderDiningOptionAsync(long orderId, DiningOption diningOption)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            if (!orderDictionary.ContainsKey(orderId))
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            orderDictionary[orderId].DiningOption = diningOption;
        }

        public async Task<long> StartNewOrderAsync()
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            long newOrderNumber;
            do
            {
                newOrderNumber = GetNewOrderId();
            }
            while (orderDictionary.ContainsKey(newOrderNumber));

            orderDictionary.Add(newOrderNumber, new Order());

            return newOrderNumber;
        }

        public async Task<CartItem> AddNewCartItemAsync(long orderId, int menuItemId)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            if (!orderDictionary.ContainsKey(orderId))
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            var orderItem = new CartItem();

            orderItem.MenuItemId = menuItemId;

            if (orderDictionary[orderId].Cart.Items.Any(x => x.MenuItemId == menuItemId))
                orderDictionary[orderId].Cart.Items = new ObservableCollection<CartItem>(orderDictionary[orderId].Cart.Items.Where(x => x.MenuItemId != menuItemId));

            orderDictionary[orderId].Cart.Items.Add(orderItem);

            return orderItem;
        }


        private long GetNewOrderId()
        {
            var newOrderId = randomNumberGenerator.NextInt64(345680, Int32.MaxValue);
            return newOrderId;
        }

        public async Task<Cart> GetCartDetails(long orderId)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            return orderDictionary[orderId].Cart;
        }

        public async Task<CartItem?> AddOrderTagsToCart(long orderId, MenuItem menuItem, MealItOptions mealItOptions, int quantity, IReadOnlyCollection<TagWithQuantity> orderTags)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            var cartItem = await this.AddNewCartItemAsync(orderId, menuItem.Id);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.MenuItem = menuItem;
                cartItem.MealItOption = mealItOptions;

                if (orderTags?.Any() == true)
                {
                    cartItem.TagWithQuantities = orderTags.ToArray();
                    cartItem.ComboType = orderTags.First().WorkFlowName;
                }

                if (menuItem != null && menuItem.OrderTags != null && menuItem.OrderTags.Any())
                    cartItem.HasOrderTags = true;

                return cartItem;
            }

            return null;
        }

        public async Task<bool> ClearCart(long orderId)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            orderDictionary[orderId].Cart.Items.Clear();

            return true;
        }

        public async Task<bool> AddQuantity(long orderId)
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            orderDictionary[orderId].Cart.Items.Clear();

            return true;
        }

        public async Task<IReadOnlyCollection<PaymentMethod>> ReadPaymentMethod()
        {
            await Task.Delay(DelayTimeInMilliSec); // Artificial delay to give the impression of work

            return new List<PaymentMethod>()
            {
                new PaymentMethod()
                {
                    Id = 1,
                    Name = "NETS",
                    //Icon = LoadImageToBase64String("\\Resources\\Images\\visa_logo.png"),
                     Icon = "\\Resources\\Images\\Nets.png",
                     Description = "NETS",
                     PaymentMode = "NETSQR"
                },
                new PaymentMethod()
                {
                    Id = 2,
                    Name = "VISA",
                    //Icon = LoadImageToBase64String("\\Resources\\Images\\flashpay_logo.png"),
                    Icon = "\\Resources\\Images\\Visa.png",
                    Description="Credit Card/Debit Card",
                    PaymentMode = "CREDITCARD"
                },
                //new PaymentMethod()
                //{
                //    Id = 3,
                //    Name = "Cash",
                //    Icon = LoadImageToBase64String("\\Resources\\Images\\paynow_logo.png"),
                //}

            };
        }

        private string LoadImageToBase64String(string image)
        {
            if (string.IsNullOrEmpty(image))
            {
                return string.Empty;
            }

            try
            {

                var path = string.Concat(System.Environment.CurrentDirectory, "\\", image);
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                return base64ImageRepresentation;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        public string GenerateFlyTicket(CartItem cartItem, RestaurantBrand restaurantBrand)
        {
            var gatewayRequest = new DineGatewayTicketRequest();

            var flyTicketRequest = new FlyTicket
            {
                TotalAmount = cartItem.Price,
                FlyTicketStatus = FlyTicketStatus.NewOrders,
                QueueNumber = ApplicationStateContext.OrderQueue
            };

            flyTicketRequest.Orders = new List<FlyOrder>();

            flyTicketRequest.Orders.Add(new FlyOrder()
            {
                MenuItemId = cartItem.MenuItemId,
                Quantity = cartItem.Quantity,
                MenuItemName = cartItem.MenuItem.Name,
                Price = cartItem.Price,
                OrderTagItems = GetOrderTagsFromCartItem(cartItem)
            });

            gatewayRequest.claimed = false;
            gatewayRequest.locationId = restaurantBrand.DineGateWayId;
            gatewayRequest.ticketNumber = string.Empty;
            gatewayRequest.response = string.Empty;
            gatewayRequest.status = string.Empty;
            gatewayRequest.deliveryContents = JsonConvert.SerializeObject(flyTicketRequest, Formatting.Indented);

            return JsonConvert.SerializeObject(gatewayRequest, Formatting.Indented);
        }

        private List<FlyOrderTagItems> GetOrderTagsFromCartItem(CartItem cartItem)
        {
            var lstFlyOrderTags = new List<FlyOrderTagItems>();

            if (cartItem.TagWithQuantities != null && cartItem.TagWithQuantities.Any())
            {
                foreach (var tag in cartItem.TagWithQuantities)
                {
                    if(cartItem.ComboType == MealItOptionHelper.MealIt)
                    {
                        lstFlyOrderTags.Add(new FlyOrderTagItems() 
                        { 
                            OrderTagId = tag.MenuItem.Id,
                            Quantity = tag.MenuItem.TotalQuantity,
                            Price = tag.MenuItem.TotalPrice,
                            TagName = tag.MenuItem.DisplayName,
                            //OrderTagGroupId = tag.MenuItem.CategorieId
                        });
                    }

                    if (cartItem.ComboType == MealItOptionHelper.AlaCarte)
                    {
                        lstFlyOrderTags.Add(new FlyOrderTagItems()
                        {
                            OrderTagId = tag.OrderTagItem.Id,
                            Quantity = tag.OrderTagItem.TotalQuantity,
                            Price = tag.OrderTagItem.Price,
                            TagName = tag.OrderTagItem.Name,
                        });
                    }
                }
            }
            return lstFlyOrderTags;
        }
    }
}
