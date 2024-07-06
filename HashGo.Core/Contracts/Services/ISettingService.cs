using HashGo.Core.Models;

namespace HashGo.Core.Contracts.Services
{
    public interface ISettingService
    {
        Task<IReadOnlyCollection<SettingCategory>> GetSettingCategoriesAsync();

        Task<QueueSetting> LoadQueueSettingAsync();

        Task<bool> StoreQueueSettingAsync(QueueSetting queueSetting);
    }
}