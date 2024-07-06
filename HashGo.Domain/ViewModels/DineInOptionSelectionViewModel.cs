using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;

namespace HashGo.Domain.ViewModels
{
    public partial class DineInOptionSelectionViewModel : BaseNavigateableViewModel<IRestaurantBrandService>
    {
        public DineInOptionSelectionViewModel(ILoggingService loggingService,
                                              IRestaurantBrandService brandService,
                                              INavigationService navigationService)
            : base(loggingService, brandService, navigationService)
        {
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(InitializeDataAsync)}() Completed.");
        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private bool CanDiningInOption() { return this.SelectedRestaurant != null; }

        [RelayCommand(CanExecute = nameof(CanDiningInOption))]
        private async Task DiningInOption()
        {
            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(DiningInOption)}() Started.");

            var parameters = new Dictionary<string, object>
            {
                { "SelectedRestaurant", this.SelectedRestaurant },
            };

            await this.NavigateToPage(Pages.RestaurantStart, parameters);

            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(DiningInOption)}() Completed.");

        }

        private bool CanTakeAwayOption() { return this.SelectedRestaurant != null; }


        [RelayCommand(CanExecute = nameof(CanTakeAwayOption))]
        private async Task TakeAwayOption()
        {
            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(TakeAwayOption)}() Started.");

            var parameters = new Dictionary<string, object>
            {
               { "SelectedRestaurant", this.SelectedRestaurant },
            };

            await this.NavigateToPage(Pages.RestaurantStart, parameters);

            this.Logger.Trace($"{nameof(DineInOptionSelectionViewModel)} : {nameof(TakeAwayOption)}() Completed.");

        }

    }
}