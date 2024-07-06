using DineGo.Core.Contracts.Services;
using DineGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DineGo.Domain.Services
{
    public class CartService : ICartService
    {
        //private readonly IRepository<CartItem> _cartItemRepository;
        //private readonly IMediaService _mediaService;
        //private readonly ICouponService _couponService;
        //private readonly bool _isProductPriceIncludeTax;
        //private readonly ICurrencyService _currencyService;
        //private readonly IStringLocalizer _localizer;

        //public CartService(IRepository<CartItem> cartItemRepository, ICouponService couponService,
        //    IMediaService mediaService, IConfiguration config, ICurrencyService currencyService, IStringLocalizerFactory stringLocalizerFactory)
        //{
        //    _cartItemRepository = cartItemRepository;
        //    _couponService = couponService;
        //    _mediaService = mediaService;
        //    _currencyService = currencyService;
        //    _isProductPriceIncludeTax = config.GetValue<bool>("Catalog.IsProductPriceIncludeTax");
        //    _localizer = stringLocalizerFactory.Create(null);
        //}

        public async Task<bool> AddToCart(long customerId, long productId, int quantity)
        {
            var addToCartResult = new Response { Success = false };

            if (quantity <= 0)
            {
                //addToCartResult.ErrorMessage = _localizer["The quantity must be larger than zero"].Value;
                addToCartResult.ErrorCode = "wrong-quantity";
                return addToCartResult;
            }

            //var cartItem = await _cartItemRepository.Query().FirstOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == customerId);
            //if (cartItem == null)
            //{
            //    cartItem = new CartItem
            //    {
            //        CustomerId = customerId,
            //        ProductId = productId,
            //        Quantity = quantity,
            //        CreatedOn = DateTimeOffset.Now
            //        //TODO add vendor id to cartitem
            //    };

            //    _cartItemRepository.Add(cartItem);
            //}
            //else
            //{
            //    cartItem.Quantity = cartItem.Quantity + quantity;
            //}

            //await _cartItemRepository.SaveChangesAsync();

            addToCartResult.Success = true;
            return addToCartResult;
        }

        // TODO separate getting product thumbnail, varation options from here
        public async Task<Cart> GetCartDetails(long customerId)
        {
            //var cartItems = await _cartItemRepository.Query().Where(x => x.CustomerId == customerId).ToListAsync();
            //if (!cartItems.Any())
            //{
            //    return null;
            //}

            var cartVm = new Cart();

            //cartVm.Items = _cartItemRepository
            //    .Query()
            //    .Include(x => x.Product).ThenInclude(p => p.ThumbnailImage)
            //    .Include(x => x.Product).ThenInclude(p => p.OptionCombinations).ThenInclude(o => o.Option)
            //    .Where(x => x.CustomerId == customerId).ToList()
            //    .Select(x => new CartItemVm(_currencyService)
            //    {
            //        Id = x.Id,
            //        ProductId = x.ProductId,
            //        ProductName = x.Product.Name,
            //        ProductPrice = x.Product.Price,
            //        ProductStockQuantity = x.Product.StockQuantity,
            //        ProductStockTrackingIsEnabled = x.Product.StockTrackingIsEnabled,
            //        IsProductAvailabeToOrder = x.Product.IsAllowToOrder && x.Product.IsPublished && !x.Product.IsDeleted,
            //        ProductImage = _mediaService.GetThumbnailUrl(x.Product.ThumbnailImage),
            //        Quantity = x.Quantity,
            //        VariationOptions = CartItemVm.GetVariationOption(x.Product)
            //    }).ToList();

            //cartVm.SubTotal = cartVm.Items.Sum(x => x.Quantity * x.ProductPrice);
            //if (!string.IsNullOrWhiteSpace(cartVm.CouponCode))
            //{
            //    var cartInfoForCoupon = new CartInfoForCoupon
            //    {
            //        Items = cartVm.Items.Select(x => new CartItemForCoupon { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
            //    };
            //    var couponValidationResult = await _couponService.Validate(customerId, cartVm.CouponCode, cartInfoForCoupon);
            //    if (couponValidationResult.Succeeded)
            //    {
            //        cartVm.Discount = couponValidationResult.DiscountAmount;
            //    }
            //    else
            //    {
            //        cartVm.CouponValidationErrorMessage = couponValidationResult.ErrorMessage;
            //    }
            //}

            return cartVm;
        }
    }
}
