using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace HashGo.Domain.ViewModels
{
    public partial class MealItSelectionViewModel : BaseWorkFlowViewModel
    {
        [ObservableProperty]
        MenuItem mealItDeal;

        [ObservableProperty]
        string mealItImageData;

        [ObservableProperty]
        string alaCartImageData;

        public MealItSelectionViewModel(ILoggingService loggingService,
            IRestaurantBrandService resturantBrandService,
                                        INavigationService navigationService,
                                        IOrderService orderService)
            : base(loggingService, resturantBrandService, navigationService, orderService)
        {
        }

        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            if(this.SelectedMenuItem != null && this.SelectedMenuItem.Combo != null && this.SelectedMenuItem.Combo.ComboGroups != null)
            {
                foreach (var comboGroup in this.SelectedMenuItem.Combo.ComboGroups)
                {
                    if (comboGroup.ComboItems != null && comboGroup.ComboItems.Any())
                    {
                        foreach (var item in comboGroup.ComboItems)
                        {
                            if (item.Name.ToUpper() == MealItOptionHelper.MealIt)
                                MealItImageData = item.Files;

                            if (item.Name.ToUpper() == MealItOptionHelper.AlaCarte)
                                AlaCartImageData = item.Files;
                        }
                    }
                }
            }

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(LoadDataAsync)}() Completed.");
        }

        private bool CanMealItOption() { return this.SelectedRestaurant != null; }

        [RelayCommand(CanExecute = nameof(CanMealItOption))]
        private async Task MealItOption()
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(MealItOption)}() Started.");

            this.SelectedMealItOption = MealItOptions.MealIt;

            await NavigateToMenuDetails(Pages.MealItWorkFlow);

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(MealItOption)}() Completed.");

        }

        private async Task NavigateToMenuDetails(Pages page)
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Started.");

            var parameters = new Dictionary<string, object>
            {
                { nameof(this.OrderId), this.OrderId },
                { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                { nameof(this.SelectedMenuItem), this.SelectedMenuItem },
                { nameof(this.SelectedMealItOption), this.SelectedMealItOption },
            };

            await NavigationService.NavigateToAsync(page.ToString(), parameters);

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToMenuDetails)}() Completed.");

        }

        private bool CanAlaCarte() { return this.SelectedRestaurant != null; }

        [RelayCommand(CanExecute = nameof(CanAlaCarte))]
        private async Task AlaCarte()
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(AlaCarte)}() Started.");

            this.SelectedMealItOption = MealItOptions.AlaCarte;

            await NavigateToMenuDetails(Pages.AlacarteWorkFlow);

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(AlaCarte)}() Completed.");

        }
        private bool CanNavigateToRestaurantHome() { return !this.IsLoading; }


        [RelayCommand(CanExecute = nameof(CanNavigateToRestaurantHome))]
        private async Task NavigateToRestaurantHome()
        {
            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToRestaurantHome)}() Started.");

            await NavigationService.NavigateToAsync(Pages.RestaurantHome.ToString());

            this.Logger.Trace($"{nameof(MealItSelectionViewModel)} : {nameof(NavigateToRestaurantHome)}() Completed.");

        }


    }
}
