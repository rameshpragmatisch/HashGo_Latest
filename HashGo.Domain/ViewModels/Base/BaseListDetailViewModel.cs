namespace DineGo.Domain.ViewModels.Base;

public abstract partial class BaseListViewModel<TItem, TPageDetail> : BaseInitializeableViewModel
	where TItem : class
	where TPageDetail : class
{
	[ObservableProperty]
	ObservableCollection<TItem> items;

	[RelayCommand]
	private async Task GoToDetails(TItem item)
	{
		await Shell.Current.GoToAsync(typeof(TPageDetail).Name, true, new Dictionary<string, object>
		{
			{ "Item", item }
		});
	}
}
