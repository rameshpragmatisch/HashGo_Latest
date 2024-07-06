using HashGo.Core.Contracts.Services;

namespace HashGo.Wpf.App.Contracts.Services;

public interface ISystemService : IApplicationService
{
    void OpenInWebBrowser(string url);
}
