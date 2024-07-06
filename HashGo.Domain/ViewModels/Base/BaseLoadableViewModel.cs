using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HashGo.Domain.ViewModels.Base
{
    public abstract partial class BaseLoadableViewModel : BaseInitializeableViewModel
    {
        [ObservableProperty]
        private bool isLoading;

        [RelayCommand(CanExecute =nameof(CanLoadData))]
        protected virtual async Task LoadData()
        {
            IsLoading = true;

            try
            {
                await LoadDataAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected virtual bool CanLoadData() { return this.IsInitialized == true && !IsLoading; }

        protected abstract Task LoadDataAsync();
    }
}