using CommunityToolkit.Mvvm.ComponentModel;

namespace DineGo.Domain.ViewModels.Base
{

    [QueryProperty(nameof(Item), "Item")]
    public abstract partial class BaseDetailDetailViewModel<T> : BaseViewModel
    {
        [ObservableProperty]
        T item;
    }
}
