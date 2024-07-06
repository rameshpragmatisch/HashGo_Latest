using HashGo.Core.Contracts.Services;
using System.Reflection.Metadata;

namespace HashGo.Core.Contracts.Views
{

    public interface INavigationService : IApplicationService
    {
        bool CanNavigateToPreviousScreen { get; }

        Task<bool> NavigateToAsync(string pageKey, object parameter = null, bool clearNavigation = false);
        
        Task<bool> NavigateToPreviousScreen(object parameter = null);
    }
}
