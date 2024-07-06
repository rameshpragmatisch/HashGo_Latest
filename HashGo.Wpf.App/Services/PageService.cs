using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Enum;
using HashGo.Wpf.App.BestTech.Views;
using HashGo.Wpf.App.Contracts.Services;
using HashGo.Wpf.App.ViewModels;
using HashGo.Wpf.App.Views;
using HashGo.Wpf.App.Views.Pages;

namespace HashGo.Wpf.App.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        //Configure(Pages.Home.ToString(), typeof(MainPage));
        Configure(Pages.TenantConnectConfiguration.ToString(), typeof(ConfigurationPage));
        Configure(Pages.Settings.ToString(), typeof(Views.Pages.SettingsPage));
        Configure(Pages.ConnectCredentials.ToString(), typeof(ConnectCredentialsPage));
        Configure(Pages.RestaurantSelection.ToString(), typeof(BrandSelectionPage));
        Configure(Pages.DiningOption.ToString(), typeof(DineInOptionSelectionPage));
        Configure(Pages.RestaurantStart.ToString(), typeof(BrandStartPage));
        Configure(Pages.RestaurantHome.ToString(), typeof(RestaurantHomePage));
        Configure(Pages.MealItSelection.ToString(), typeof(MealItSelectionPage));
        Configure(Pages.MealItWorkFlow.ToString(), typeof(MealItWorkFlowPage));
        Configure(Pages.AlacarteWorkFlow.ToString(), typeof(AlacarteWorkFlowPage));
        Configure(Pages.ItemAddedToCart.ToString(), typeof(ItemAddedToCartPage));
        Configure(Pages.CartView.ToString(), typeof(CartViewPage));
        Configure(Pages.PaymentMethod.ToString(), typeof(PaymentMethodPage));
        Configure(Pages.OrderConfirmation.ToString(), typeof(OrderConfirmationPage));
        Configure(Pages.PrinterSetting.ToString(), typeof(PrinterSettingsPage));

        //BestTech views
        Configure(Pages.RestaurantStartup.ToString(), typeof(RestaurantStartupPage));
        Configure(Pages.DineDateSelect.ToString(), typeof(ConfirmDineDatePage));
        Configure(Pages.CustomerDetails.ToString(), typeof(CustomerDetailsPage));
        Configure(Pages.ProductSelection.ToString(), typeof(ProductSelectionPage));
        Configure(Pages.Addons.ToString(), typeof(AddOnsSelectionPage));
        Configure(Pages.Payment.ToString(), typeof(PaymentsPage));
        Configure(Pages.Enquiries.ToString(), typeof(EnquiriesPage));
        Configure(Pages.HashGoSettings.ToString(), typeof(BestTech.Views.SettingsPage));
        Configure(Pages.QRPayment.ToString(), typeof(QRPaymentPage));
        Configure(Pages.ProcessingPayment.ToString(), typeof(ProcessingPaymentPage));
        Configure(Pages.PurchaseSucceded.ToString(), typeof(PurchaseConfirmedPage));
        Configure(Pages.PurchaseFailed.ToString(), typeof(PurchaseFailedPage));
        Configure(Pages.ItemAdded.ToString(), typeof(ItemAddedPage));

    }

    public Type GetPageType(string key)
    {
        Type pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    public Page GetPage(string key)
    {
        var pageType = GetPageType(key);
        return _serviceProvider.GetService(pageType) as Page;
    }

    public bool Configure(string key, Type type)
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);

            return true;
        }
    }
}
