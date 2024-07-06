using HashGo.Core.Contracts.Services;

namespace HashGo.Wpf.App.Contracts.Services;

public interface IPersistAndRestoreService : IApplicationService
{
    void RestoreData();

    void PersistData();
}
