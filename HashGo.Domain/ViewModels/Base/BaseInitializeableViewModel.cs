using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace HashGo.Domain.ViewModels.Base
{
    public abstract partial class BaseInitializeableViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool? isInitialized = null;

        [ObservableProperty]
        private bool isInitializing;

        [RelayCommand(CanExecute = nameof(CanInitializeData))]
        protected virtual async Task OnInitializeData()
        {
            IsInitializing = true;

            try
            {
                await InitializeDataAsync();
            }
            finally
            {
                IsInitializing = false;
                IsInitialized = true;
            }
        }

        protected bool CanInitializeData() { return !this.IsInitialized.HasValue && !this.IsInitializing; }

        protected abstract Task InitializeDataAsync();
    }
}
