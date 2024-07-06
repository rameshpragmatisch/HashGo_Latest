using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Domain.ViewModels;

public partial class ConfigurationViewModel : BaseNavigateableViewModel<ITenantConnectStoreService>
{
    private readonly IProductDetailStoreService productDetailService;
    private readonly IProductService productService;

    [ObservableProperty] private IEnumerable<TenantConnect> items;

    public ConfigurationViewModel(ILoggingService loggingService,
                                  IProductService productService,
                                  ITenantConnectStoreService service,
                                  INavigationService navigationService,
                                  IProductDetailStoreService productDetailService)
        : base(loggingService, service, navigationService)
    {
        this.productService = productService;
        this.productDetailService = productDetailService;
    }

    protected override async Task InitializeDataAsync()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(InitializeDataAsync)}() Started.");

        await LoadDataAsync();
        

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(LoadDataAsync)}() Completed.");

    }

    protected override async Task LoadDataAsync()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(LoadDataAsync)}() Started.");

        var itemFromService = await BrandService.ReadAllAsync();
        Items = new ObservableCollection<TenantConnect>(itemFromService);

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(LoadDataAsync)}() Completed.");

    }

    [RelayCommand]
    private async Task AddNewConnect()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(AddNewConnect)}() Started.");

        var newConnectItem = new TenantConnect();

        var parameters = new Dictionary<string, object>
        {
            { "SelectedConnectItem", newConnectItem },
            { "IsNewConnectItem", true }
        };

        await this.NavigateToPage(Pages.ConnectCredentials, parameters);

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(AddNewConnect)}() Completed.");

    }

    [RelayCommand]
    private async Task EditConnectItem(TenantConnect connectItem)
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(EditConnectItem)}({nameof(connectItem)} : {connectItem}) Started.");


        var parameters = new Dictionary<string, object>
        {
            { "SelectedConnectItem", connectItem }
        };

        await this.NavigateToPage(Pages.ConnectCredentials, parameters);

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(EditConnectItem)}() Completed.");

    }

    [RelayCommand]
    private async Task DeleteConnectItem(TenantConnect connectItem)
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(DeleteConnectItem)}({nameof(connectItem)} : {connectItem}) Started.");

        try
        {
            this.IsLoading = true;

            bool relevantProductRemoved = false;

            var allProductItems =
                    await productDetailService.FindAllAsync(a => a.TenantUniqueKey.Equals(connectItem.TenantUniqueKey));

            foreach (var productItem in allProductItems)
            {
                relevantProductRemoved = this.productDetailService.Remove(productItem);
            }

            if (relevantProductRemoved &&
                this.BrandService.Remove(connectItem))
            {
                await this.LoadDataAsync();
            }
        }
        finally
        {
            this.IsLoading = false;
        }

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(DeleteConnectItem)}() Completed.");
    }

    [RelayCommand]
    private async Task SyncConnect()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(SyncConnect)}() Started.");

        try
        {
            IsLoading = true;

            await Task.Run(SyncConnectWithDb);
        }
        finally
        {
            IsLoading = false;
        }

        var allRefreshItems = await productDetailService.FindAllAsync(a => a.Id > 0);
        if (allRefreshItems.Any())
        {
            await MoveToBrandPage(allRefreshItems);
        }

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(SyncConnect)}() Completed.");
    }

    private async Task SyncConnectWithDb()
    {
        foreach (var connectItem in Items)
        {
            var item = new ProductDetail
            {
                TenantUniqueKey = connectItem.TenantUniqueKey
            };
            var allProductItems =
                await productDetailService.FindAllAsync(a => a.TenantUniqueKey.Equals(connectItem.TenantUniqueKey));
            if (allProductItems.Any()) item = allProductItems.First();
            item.TenantUniqueKey = connectItem.TenantUniqueKey;
            item.DeviceDetails =
                productService.GetDeviceDetail(connectItem.Url, connectItem.TenantId, connectItem.DeviceId);
            item.ScreenMenuDetails =
                productService.GetScreenMenu(connectItem.Url, connectItem.TenantId, connectItem.LocationId);
            item.OrderTagDetails =
                productService.GetOrderTags(connectItem.Url, connectItem.TenantId, connectItem.LocationId);
            item.LocationItemDetails =
                productService.GetItemsForLocation(connectItem.Url, connectItem.TenantId, connectItem.LocationId);

            await productDetailService.AddOrUpdateSync(item);
        }
    }

    private async Task MoveToBrandPage(IReadOnlyCollection<ProductDetail> allProductItems)
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(MoveToBrandPage)}({nameof(allProductItems)} : {allProductItems}) Started.");

        var parameters = new Dictionary<string, object>
        {
            { "TenantConnects", Items }
        };

        await this.NavigateToPage(Pages.RestaurantSelection, parameters);

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(MoveToBrandPage)}() Completed.");

    }

    [RelayCommand]
    private async Task NavigateToSettings()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Started.");

        await this.NavigateToPage(Pages.Settings, Array.Empty<object>());

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Completed.");

    }

    [RelayCommand]
    private async Task NavigateToPrinterSettings()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Started.");

        await this.NavigateToPage(Pages.PrinterSetting, Array.Empty<object>());

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Completed.");

    }

    [RelayCommand]
    private async Task NavigateToBrandPage()
    {
        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Started.");

        await this.NavigateToPage(Pages.RestaurantSelection, Array.Empty<object>());

        this.Logger.Trace($"{nameof(ConfigurationViewModel)} : {nameof(NavigateToSettings)}() Completed.");

    }
}