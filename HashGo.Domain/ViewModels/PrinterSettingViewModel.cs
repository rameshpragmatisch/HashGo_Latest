using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.ViewModels
{
    public partial class PrinterSettingViewModel : BaseNavigateableViewModel<ITenantConnectStoreService>
    {
        public PrinterSettingViewModel(ILoggingService loggingService,
                                          ITenantConnectStoreService service,
                                          INavigationService navigationService)
           : base(loggingService, service, navigationService)
        {
        }

        protected async override Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected async override Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(LoadDataAsync)}() Started.");


            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        [RelayCommand]
        private async void NavigateToConfigurationPage()
        {
            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToConfigurationPage)}() Started.");

            var navigated = await this.NavigateToPage(Pages.TenantConnectConfiguration, Array.Empty<object>());

            this.Logger.Trace($"{nameof(BrandSelectionViewModel)} : {nameof(NavigateToConfigurationPage)}() Completed.");

        }
    }
}
