using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Domain.ViewModels;
using HashGo.Wpf.App.Contracts.Activation;
using HashGo.Wpf.App.Contracts.Services;
using HashGo.Wpf.App.Contracts.Views;
using HashGo.Wpf.App.ViewModels;

using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;

namespace HashGo.Wpf.App.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly IPersistAndRestoreService _persistAndRestoreService;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly ILoggingService _loggingService;
    private IShellWindow _shellWindow;
    private bool _isInitialized;

    public ApplicationHostService(IServiceProvider serviceProvider, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IPersistAndRestoreService persistAndRestoreService, ILoggingService loggingService)
    {
        _serviceProvider = serviceProvider;
        _activationHandlers = activationHandlers;
        _navigationService = navigationService;
        _persistAndRestoreService = persistAndRestoreService;
        _loggingService = loggingService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize services that you need before app activation
        await InitializeAsync();

        await HandleActivationAsync();

        // Tasks after activation
        await StartupAsync();
        _isInitialized = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _persistAndRestoreService.PersistData();
        await Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _persistAndRestoreService.RestoreData();
            await Task.CompletedTask;
        }
    }

    private async Task StartupAsync()
    {
        if (!_isInitialized)
        {
            await Task.CompletedTask;
        }
    }

    private async Task HandleActivationAsync()
    {
        this._loggingService.Trace($"{nameof(ApplicationHostService)} : {nameof(HandleActivationAsync)}() Started.");

        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle());

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync();
        }


        if (!App.Current.Windows.OfType<IShellWindow>().Any())
        {
            // Default activation that navigates to the apps default page
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;

            if (_navigationService is IFrameNavigationService frameNavigationService)
            {

                frameNavigationService.Initialize(_shellWindow.GetNavigationFrame());
            }
            _shellWindow.ShowWindow();

            await NavigateToDefaultPage();
        }

        this._loggingService.Trace($"{nameof(ApplicationHostService)} : {nameof(HandleActivationAsync)}() Completed.");
    }

    private async Task NavigateToDefaultPage()
    {
        await _navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
    }

    //private async Task NavigateToDefaultPage()
    //{
    //    await _navigationService.NavigateToAsync(Pages.TenantConnectConfiguration.ToString());
    //    //IProductDetailStoreService productDetailService = _serviceProvider.GetService(typeof(IProductDetailStoreService)) as IProductDetailStoreService;
    //    //var allProductItems = await productDetailService.FindAllAsync(a => a.Id > 0);
    //    //if (allProductItems.Any())
    //    //{
    //    //    ITenantConnectStoreService connectStoreService = _serviceProvider.GetService(typeof(ITenantConnectStoreService)) as ITenantConnectStoreService;
    //    //    var itemFromService = await connectStoreService.ReadAllAsync();

    //    //    var parameters = new Dictionary<string, object>
    //    //            {
    //    //                { "TenantConnects", itemFromService.ToList() }
    //    //            };

    //    //    await _navigationService.NavigateToAsync(Pages.RestaurantSelection.ToString(), parameters);
    //    //}
    //    //else
    //    //{
    //    //    await _navigationService.NavigateToAsync(Pages.TenantConnectConfiguration.ToString());
    //    //}
    //}
}
