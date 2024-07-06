using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Domain.Contracts.Repository;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Domain.Repository;
using HashGo.Domain.Services;
using HashGo.Domain.Services.Store;
using HashGo.Domain.ViewModels;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Services;
using HashGo.Infrastructure.Setting;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.Popups;
using HashGo.Wpf.App.BestTech.ViewModels;
using HashGo.Wpf.App.BestTech.ViewModels.Popups;
using HashGo.Wpf.App.BestTech.Views;
using HashGo.Wpf.App.Contracts.Services;
using HashGo.Wpf.App.Contracts.Views;
using HashGo.Wpf.App.Models;
using HashGo.Wpf.App.Services;
using HashGo.Wpf.App.ViewModels;
using HashGo.Wpf.App.Views;
using HashGo.Wpf.App.Views.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NLog;
using Prism.Events;

namespace HashGo.Wpf.App;

// For more information about application lifecycle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

// WPF UI elements use language en-US by default.
// If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
// Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
public partial class App : Application
{
    private IHost _host;
    private ILoggingService _logger;

    public T GetService<T>()
        where T : class
        => _host.Services.GetService(typeof(T)) as T;

    public App()
    {
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration(c =>
                {
                    c.SetBasePath(appLocation);
                })
                .ConfigureServices(ConfigureServices)
                .Build();

            LogManager.Configuration.Variables["LogDir"] = LocalSetting.LogPath;

            this._logger = GetService<ILoggingService>();
            this._logger.Info("Application Starting");

            using (var context = new HashGoCacheContext())
            {
                var dbCreated = await context.Database.EnsureCreatedAsync();
                if (dbCreated)
                {
                    this._logger.Info("Database Created");
                }
            }

            await _host.StartAsync();
        }
        catch (Exception exception)
        {
            this._logger.TraceException(exception);
        }
       
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // TODO: Register your services, viewmodels and pages here

        // App Host
        services.AddHostedService<ApplicationHostService>();

        //services.AddSingleton<HashGoCacheContext>();

        #region Services

        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IRetailConnectService, RetailConnectService>();
        services.AddSingleton<IRestaurantBrandService, RestaurantBrandService>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<OrderService>();
        services.AddSingleton<IOrderService>(s => s.GetService<OrderService>());
        services.AddSingleton<ICartService>(s => s.GetService<OrderService>());

        services.AddSingleton<IViewService, ViewService>();
        services.AddSingleton<IIocService, IocService>();


        services.AddSingleton<ITenantConnectStoreService, TenantConnectionStoreService>();
        services.AddSingleton<IProductDetailStoreService, ProductDetailStoreService>();
        services.AddSingleton<IQueueSettingStoreService, QueueSettingStoreService>();
        services.AddSingleton<IEventAggregator, EventAggregator>();


        #endregion

        // Views and ViewModels
        services.AddSingleton<IShellWindow, ShellWindow>();
        services.AddSingleton<ShellViewModel>();

        //services.AddSingleton<MainViewModel>();
        //services.AddSingleton<MainPage>();

        services.AddSingleton<ConfigurationViewModel>();
        services.AddSingleton<ConfigurationPage>();

        services.AddSingleton<PrinterSettingViewModel>();
        services.AddSingleton<PrinterSettingsPage>();

        services.AddSingleton<ConnectCredentialsViewModel>();
        services.AddSingleton<ConnectCredentialsPage>();

        services.AddSingleton<BrandSelectionViewModel>();
        services.AddSingleton<BrandSelectionPage>();

        services.AddSingleton<DineInOptionSelectionPage>();
        services.AddSingleton<DineInOptionSelectionViewModel>();

        services.AddSingleton<BrandStartPage>();
        services.AddSingleton<BrandStartViewModel>();

        services.AddSingleton<RestaurantHomePage>();
        services.AddSingleton<RestaurantHomeViewModel>();

        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<Views.Pages.SettingsPage>();

        services.AddTransient<Views.Views.RestaurantMenuView>();
        services.AddTransient<BrandMenuViewModel>();

        services.AddTransient<MealItSelectionPage>();
        services.AddTransient<MealItSelectionViewModel>();

        services.AddTransient<MealItWorkFlowPage>();
        services.AddTransient<MenuItemDetailViewModel>();

        services.AddTransient<AlacarteWorkFlowPage>();
        services.AddTransient<MenuItemDetailViewModel>();

        services.AddTransient<ItemAddedToCartPage>();
        services.AddTransient<ItemAddedToCartViewModel>();

        services.AddTransient<CartViewPage>();
        services.AddTransient<CartViewViewModel>();

        services.AddTransient<PaymentMethodPage>();
        services.AddTransient<PaymentMethodViewModel>();

        services.AddTransient<OrderConfirmationPage>();
        services.AddTransient<OrderConfirmationViewModel>();

        services.AddSingleton<Views.Views.QueueSettingsView>();


        services.AddSingleton<ITenantConnectRepository, TenantConnectRepository>();
        services.AddSingleton<IProductDetailRepository, ProductDetailRepository>();
        services.AddSingleton<IQueueSettingRepository, QueueSettingRepository>();
        services.AddSingleton<ISettingService, SettingService>();

        //Best tech
        services.AddSingleton<RestaurantStartupPage>();
        services.AddSingleton<RestaurantStartupPageViewModel>();
        services.AddSingleton<ConfirmDineDatePage>();
        services.AddSingleton<ConfirmDineDatePageViewModel>();
        services.AddSingleton<CustomerDetailsPage>();
        services.AddSingleton<CustomerDetailsPageViewModel>();
        services.AddSingleton<ProductSelectionPage>();
        services.AddSingleton<ProductSelectionPageViewModel>();
        services.AddSingleton<AddOnsSelectionPage>();
        services.AddSingleton<AddOnsSelectionPageViewmodel>();
        services.AddSingleton<PaymentsPage>();
        services.AddSingleton<PaymentsPageViewModel>();
        services.AddSingleton<SharedDataService>();
        services.AddSingleton<IPopupService, PopupService>();
        services.AddSingleton<EnquiriesPage>();
        services.AddSingleton<EnquiriesPageViewModel>();
        services.AddSingleton<BestTech.Views.SettingsPage>();
        services.AddSingleton<SettingsPageViewModel>();
        services.AddTransient<ConfirmCustomerDetailsPopup>();
        services.AddTransient<ConfirmCustomerDetailsPopupViewModel>();
        services.AddTransient<ResetOrderPopup>();
        services.AddSingleton<QRPaymentPage>();
        services.AddSingleton<QRPaymentPageViewModel>();
        services.AddSingleton<ProcessingPaymentPage>();
        services.AddSingleton<PurchaseConfirmedPage>();
        services.AddSingleton<PurchaseFailedPage>();
        services.AddSingleton<ItemAddedPage>();
        services.AddTransient<VirtualKeyboard>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        this._logger.Info($"Application Exit with {nameof(e.ApplicationExitCode)}:{e.ApplicationExitCode} ");

        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        this._logger.TraceException(e.Exception);
        e.Handled = true;
        Application.Current.Shutdown();
    }
}
