using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Models;

namespace HashGo.Domain.ViewModels
{
    public partial class SettingsViewModel : BaseNavigateableViewModel<ISettingService>
    {
        IViewService viewService;

        [ObservableProperty]
        IEnumerable<SettingCategory> settingCategories;

        [ObservableProperty]
        SettingCategory selectedCategory;
        
        [ObservableProperty]
        IView viewContent;

        [ObservableProperty]
        QueueSetting queueSetting;

        private readonly IQueueSettingStoreService queueSettingDetailService;
        private readonly ISettingService settingService;

        public SettingsViewModel(ILoggingService loggingService, ISettingService settingsService, INavigationService navigationService, IViewService viewService, IQueueSettingStoreService queueSettingService) 
            : base(loggingService, settingsService, navigationService)
        {
            this.viewService = viewService;
            this.settingService = settingsService;
            this.queueSettingDetailService = queueSettingService;
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

            this.SettingCategories = await this.BrandService.GetSettingCategoriesAsync();
            this.SelectedCategory = this.SettingCategories?.FirstOrDefault();

            var qSDataFromDB = await queueSettingDetailService.ReadAllAsync();

            if (qSDataFromDB != null && qSDataFromDB.Any())
            {
                var qsData = qSDataFromDB.First();

                // To do - Bind Queue settings back to UI
                //this.QueueSetting = new QueueSetting()
                //{
                //    StartNumber = qsData.StartNumber,
                //    EndNumber = qsData.EndNumber,
                //    QueuePrefix = qsData.Prefix,
                //    QueueSuffix = qsData.Suffix,
                //    EnableQueue = qsData.IsEnabled,
                //    ResetQueue = qsData.IsReset
                //};
            }

            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        async partial void OnSelectedCategoryChanging(SettingCategory? oldValue, SettingCategory newValue)
        {
            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(OnSelectedCategoryChanging)}() Started.");

            if (oldValue != newValue && newValue != null)
            {
                var view = GetView(newValue.View.ToString(), this.viewService, this.Logger);
                if (view is IHasDataContext hasDataContext)
                {
                    this.QueueSetting = await this.BrandService.LoadQueueSettingAsync();
                    this.ViewContent = view;

                    hasDataContext.DataContext = this.QueueSetting;
                }
            }

            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(OnSelectedCategoryChanging)}() Completed.");

        }

        private bool CanSaveSettings() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanSaveSettings))]
        private async Task SaveSettings()
        {
            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(SaveSettings)}() Started.");

            var queueSetting = new QueueSettings()
            {
                StartNumber = this.QueueSetting.StartNumber,
                EndNumber = this.QueueSetting.EndNumber,
                Prefix = this.QueueSetting.QueuePrefix,
                Suffix = this.QueueSetting.QueueSuffix,
                IsEnabled = this.QueueSetting.EnableQueue,
                IsReset = this.QueueSetting.ResetQueue
            };

            var qSDataFromDB = await queueSettingDetailService.ReadAllAsync();

            if (qSDataFromDB != null && qSDataFromDB.Any())
            {
                foreach (var qsData in qSDataFromDB)
                    queueSettingDetailService.Remove(qsData);
            }

            var value = await queueSettingDetailService.AddOrUpdateSync(queueSetting);

            if (value != null)
            {
                this.Logger.Trace($"{nameof(ConnectCredentialsViewModel)} : {nameof(SaveSettings)}() Added.");

                await this.NavigateToPreviousScreenCommand.ExecuteAsync(null);
            }

            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(SaveSettings)}() Completed.");
        }

        private bool CanCancelEdit() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanCancelEdit))]
        private async Task CancelEdit()
        {
            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(CancelEdit)}() Started.");

            await this.NavigateToPreviousScreenCommand.ExecuteAsync(null);

            this.Logger.Trace($"{nameof(SettingsViewModel)} : {nameof(CancelEdit)}() Completed.");
        }
    }
}
