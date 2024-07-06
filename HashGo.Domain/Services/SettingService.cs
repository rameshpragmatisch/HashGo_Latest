using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Models;

namespace HashGo.Domain.Services
{
    public class SettingService : ISettingService
    {

        public Task<IReadOnlyCollection<SettingCategory>> GetSettingCategoriesAsync()
        {
            var settingCategories = new List<SettingCategory>();

            var queueSetting = new SettingCategory()
            {
                Id = 1,
                Name = "Queue",
                Description = "Queue Settings",
                HelperText = "Customise app queue settings to improve customer statisfaction",
                View = Core.Enum.Views.QueueSettings,
            };

            settingCategories.Add(queueSetting);

            return Task.FromResult<IReadOnlyCollection<SettingCategory>>(settingCategories);
        }

        public Task<QueueSetting> LoadQueueSettingAsync()
        {

            var queueSetting = new QueueSetting();

           return  Task.FromResult<QueueSetting>(queueSetting);
        }

        public Task<bool> StoreQueueSettingAsync(QueueSetting queueSetting)
        {
            return Task.FromResult<bool>(false);
        }
    }
}
