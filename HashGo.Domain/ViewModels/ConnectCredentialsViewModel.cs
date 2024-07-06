using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;

namespace HashGo.Domain.ViewModels
{

    public partial class ConnectCredentialsViewModel : BaseNavigateableViewModel<ITenantConnectStoreService>
    {
        public ConnectCredentialsViewModel(ILoggingService loggingService,
                                           ITenantConnectStoreService service,
                                           INavigationService navigationService)
            : base(loggingService, service, navigationService)
        {
        }

        [ObservableProperty]
        private TenantConnect selectedConnectItem;

        [ObservableProperty]
        private bool isNewConnectItem;

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await LoadDataAsync();

            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }


        [RelayCommand]
        private async Task SaveConnect(TenantConnect? connectItem)
        {
            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(SaveConnect)}() Started.");

            if (connectItem != null && !string.IsNullOrEmpty(connectItem.Url))
            {
                try
                {
                    if (connectItem.Id > 0)
                    {
                        var result = this.BrandService.Update(connectItem);
                        if (result)
                        {
                            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(SaveConnect)}() Updated.");

                            await this.NavigateToPreviousScreen();
                        }
                    }
                    else
                    {
                        var value = await this.BrandService.AddAsync(connectItem);
                        if (value != null)
                        {
                            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(SaveConnect)}() Added.");

                            await this.NavigateToPreviousScreen();
                        }
                    }
                }
                catch (Exception ex) 
                {
                    this.Logger.TraceException(ex);
                }
            }

            IsNewConnectItem = false;

            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(SaveConnect)}() Completed.");
        }

        [RelayCommand]
        private async Task CancelEdit()
        {
            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(CancelEdit)}() Started.");

            await this.NavigateToPreviousScreen();

            this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(CancelEdit)}() Completed.");
        }
    }
}