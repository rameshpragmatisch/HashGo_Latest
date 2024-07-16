using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure
{
    public static class HashGoAppSettings
    {
        public static string Url { get; set; }
        public static string Tenant { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string TenantId { get; set; }
        public static string DeviceId { get; set; }
        public static string LocationId { get; set; }
        public static string SortOrder { get; set; }
        public static string PaymentScreenVisibleDelay { get; set; } = "10";
        public static string NETSPort { get; set; }

        private static AppSettingsSection appSettingSection;
        private static Configuration configFile;
        private readonly static string hashGoPath;

        static HashGoAppSettings()
        {
            string? documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            hashGoPath = $"{documentPath}/HashGo/HashGo.dll.config";
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = hashGoPath;
            configFile = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            appSettingSection = (AppSettingsSection)configFile.GetSection("appSettings");
        }

        public static void LoadSettings()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = hashGoPath;
            configFile = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            appSettingSection = (AppSettingsSection)configFile.GetSection("appSettings");

            Url = appSettingSection.Settings[nameof(Url)]?.Value;
            Tenant = appSettingSection.Settings[nameof(Tenant)]?.Value;
            User = appSettingSection.Settings[nameof(User)]?.Value;
            Password = appSettingSection.Settings[nameof(Password)]?.Value;
            TenantId = appSettingSection.Settings[nameof(TenantId)]?.Value;
            DeviceId = appSettingSection.Settings[nameof(DeviceId)]?.Value;
            LocationId = appSettingSection.Settings[nameof(LocationId)]?.Value;
            SortOrder = appSettingSection.Settings[nameof(SortOrder)]?.Value;

            PaymentScreenVisibleDelay = appSettingSection.Settings[nameof(PaymentScreenVisibleDelay)]?.Value;
            NETSPort = appSettingSection.Settings[nameof(NETSPort)]?.Value;

        }

        public static void SaveSettings()
        {
            AddOrUpdateAppSettings(nameof(Url), Url);
            AddOrUpdateAppSettings(nameof(Tenant), Tenant);
            AddOrUpdateAppSettings(nameof(User), User);
            AddOrUpdateAppSettings(nameof(Password), Password);
            AddOrUpdateAppSettings(nameof(TenantId), TenantId);
            AddOrUpdateAppSettings(nameof(DeviceId), DeviceId);
            AddOrUpdateAppSettings(nameof(LocationId), LocationId);
            AddOrUpdateAppSettings(nameof(SortOrder), SortOrder);
            AddOrUpdateAppSettings(nameof(PaymentScreenVisibleDelay), PaymentScreenVisibleDelay);
            AddOrUpdateAppSettings(nameof(NETSPort), NETSPort);

            LoadSettings();
        }

        private static void AddOrUpdateAppSettings(string key, string? value)
        {
            try
            {
                KeyValueConfigurationCollection? settings = appSettingSection.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                //NLogger.Error(ex);
            }
        }
    }
}
