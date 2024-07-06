using HashGo.Core.Contracts.View;

namespace HashGo.Core.Contracts.Services
{

    public interface IViewService : IApplicationService
    {
        Type GetViewType(string key);

        IView GetView(string key);
    }
}