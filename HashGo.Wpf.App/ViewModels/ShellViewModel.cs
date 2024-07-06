using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Domain.ViewModels.Base;
using HashGo.Wpf.App.Contracts.Services;

namespace HashGo.Wpf.App.ViewModels
{

    public class ShellViewModel : BaseViewModel
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly ILoggingService _Logger;

        private RelayCommand _goBackCommand;
        private RelayCommand _loadedCommand;
        private RelayCommand _unloadedCommand;

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnNavigateToPreviousScreen, CanNavigateToPreviousScreen));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(ILoggingService loggingService, INavigationService navigationService)
        {
            this._Logger = loggingService;

            if (navigationService is IFrameNavigationService service)
            {
                _navigationService = service;
            }
        }

        private void OnLoaded()
        {
            _Logger.Trace($"{this.GetType().Name} : {nameof(OnLoaded)}() Started.");
            
            _navigationService.Navigated += OnNavigated;

            _Logger.Trace($"{this.GetType().Name} : {nameof(OnLoaded)}() Completed.");

        }

        private void OnUnloaded()
        {
            _Logger.Trace($"{this.GetType().Name} : {nameof(OnUnloaded)}() Started.");

            _navigationService.Navigated -= OnNavigated;

            _Logger.Trace($"{this.GetType().Name} : {nameof(OnUnloaded)}() Completed.");

        }

        private bool CanNavigateToPreviousScreen()
        {
            _Logger.Trace($"{this.GetType().Name} : {nameof(OnUnloaded)}() Started.");
            
            var result = _navigationService.CanNavigateToPreviousScreen;

            _Logger.Trace($"{this.GetType().Name} : {nameof(OnUnloaded)}() Completed with result: {result}.");

            return result;
            
        }

        private async void OnNavigateToPreviousScreen()
        {
            _Logger.Trace($"{this.GetType().Name} : {nameof(OnNavigateToPreviousScreen)}() Started.");

            await _navigationService.NavigateToPreviousScreen();

            _Logger.Trace($"{this.GetType().Name} : {nameof(OnNavigateToPreviousScreen)}() Completed.");

        }

        private void OnNavigated(object sender, string viewModelName)
        {
            _Logger.Trace($"{this.GetType().Name} : {nameof(OnNavigated)}({nameof(sender)}:{sender}, {nameof(viewModelName)}:{viewModelName} ) Started.");

            GoBackCommand.NotifyCanExecuteChanged();

            _Logger.Trace($"{this.GetType().Name} : {nameof(OnNavigated)}() Completed.");

        }
    }
}