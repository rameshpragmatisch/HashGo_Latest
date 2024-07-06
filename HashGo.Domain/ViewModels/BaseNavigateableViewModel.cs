using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;

namespace HashGo.Domain.ViewModels
{
    public abstract partial class BaseNavigateableViewModel<T> : BaseDataProviderViewModel<T>
    {
        [ObservableProperty]
        private RestaurantBrand? selectedRestaurant;

        readonly INavigationService navigationService;

        protected BaseNavigateableViewModel(ILoggingService loggingService,
                                            T brandService,
                                            INavigationService navigationService) 
            : base(loggingService, brandService)
        {
            this.navigationService = navigationService;
        }

        protected static IView? GetView(string viewKey, IViewService viewService, ILoggingService loggingService)
        {
            loggingService.Trace($" BaseNavigateableViewMode<T> : {nameof(GetView)}({nameof(viewKey)} : {viewKey}) Started.");


            if (string.IsNullOrEmpty(viewKey))
            {
                throw new ArgumentException($"'{nameof(viewKey)}' cannot be null or empty.", nameof(viewKey));
            }

            var viewType = viewService.GetViewType(viewKey);
            IView view = null;

            if (viewType != null)
            {
                view = viewService.GetView(viewKey);
            }

            loggingService.Trace($" BaseNavigateableViewMode<T> : {nameof(GetView)}() Completed with {nameof(view)} : {view}.");

            return view;
        }

        protected INavigationService NavigationService { get { return navigationService; } }

        public virtual bool CanNavigateToPreviousScreen() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanNavigateToPreviousScreen))]
        public async Task NavigateToPreviousScreen()
        {
            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToPreviousScreen)}() Started.");

            var navigated = await NavigationService.NavigateToPreviousScreen();

            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToPreviousScreen)}() Completed with {nameof(navigated)} : {navigated}.");
        }

        public virtual bool CanNavigateToHomeScreen() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanNavigateToHomeScreen))]
        public async Task NavigateToHomeScreen()
        {
            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToHomeScreen)}() Started.");

            var navigated = await NavigationService.NavigateToAsync(Pages.RestaurantHome.ToString(), Array.Empty<object>(), true);

            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToHomeScreen)}() Completed with {nameof(navigated)} : {navigated}.");
        }


        public virtual bool CanNavigateToBrandSelection() { return !this.IsLoading; }

        [RelayCommand(CanExecute = nameof(CanNavigateToBrandSelection))]
        public async Task NavigateToBrandSelection()
        {
            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToBrandSelection)}() Started.");

            var navigated = await NavigationService.NavigateToAsync(Pages.RestaurantSelection.ToString(), Array.Empty<object>(), true);

            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToBrandSelection)}() Completed with {nameof(navigated)} : {navigated}.");
        }

        protected async virtual Task<bool> NavigateToPage(Pages page, object parameter) 
        {
            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToPage)}({nameof(page)} : {page}, {nameof(parameter)} : {parameter} ) Started.");

            var navigated = await NavigationService.NavigateToAsync(page.ToString(), parameter);

            this.Logger.Trace($"{this.GetType().Name} : {nameof(NavigateToPage)}() Completed with {nameof(navigated)} : {navigated}.");

            return navigated;
        }


    }
}
