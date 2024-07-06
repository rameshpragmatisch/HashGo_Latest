using HashGo.Core.Contracts.Services;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Contracts.Services;

public interface IPageService : IApplicationService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
