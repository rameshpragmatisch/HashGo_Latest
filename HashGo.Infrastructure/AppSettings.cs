using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure
{
    public class AppSettings
    {
        public static string? Url { get; set; }
        public static string? Tenant { get; set; }
        public static string? User { get; set; }
        public static string? Password { get; set; }
        public static string? TenantId { get; set; }
        public static string? DeviceId { get; set; }
        public static string? LocationId { get; set; }
        public static string? PrintFontFamily { get; set; }
        public static string? PrinterName { get; set; }
        public static string? PrinterCodePage { get; set; }
        public static string? PrinterCharPerLine { get; set; }
        public static string? NetsPort { get; set; }
        public static string? BackgroundImage { get; set; }
        public static bool OpenSchedulesShow { get; set; }
        public static bool ShowLanguageSelection { get; set; }
        public static bool ShowMemberButton { get; set; }

        public static double MenuBackgroundTransparency { get; set; }

        public static string? AscentisApiLink { get; set; }
        public static string? AscentisCampaign { get; set; }
        public static string? AscentisCashierIdentifier { get; set; }
        public static string? AscentisDatabase { get; set; }
        public static string? AscentisEnquiryCode { get; set; }
        public static string? AscentisMembershipType { get; set; }
        public static string? AscentisOutletCode { get; set; }
        public static string? AscentisPassword { get; set; }
        public static string? AscentisPosIdentifier { get; set; }
        public static string? AscentisUser { get; set; }

        public static int Decimals { get; set; } = 2;
        public static bool QueueOrder { get; set; }

        public static bool AscentisLog { get; set; }

        public static bool ShowAscentis { get; set; }
        public static string? AppliedTerminal { get; set; }

        public static string? CurrencySymbol { get; set; }

        private static AppSettingsSection appSettingSection;
        private static Configuration configFile;
        private readonly static string dineGoPath;


        static AppSettings()
        {
            string? documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dineGoPath = $"{documentPath}/DineGo/DineGo.dll.config";
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = dineGoPath;
            configFile = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            appSettingSection = (AppSettingsSection)configFile.GetSection("appSettings");
        }

        public static void LoadSettings()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = dineGoPath;
            configFile = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            appSettingSection = (AppSettingsSection)configFile.GetSection("appSettings");

            Url = appSettingSection.Settings[nameof(Url)]?.Value;
            Tenant = appSettingSection.Settings[nameof(Tenant)]?.Value;
            User = appSettingSection.Settings[nameof(User)]?.Value;
            Password = appSettingSection.Settings[nameof(Password)]?.Value;
            TenantId = appSettingSection.Settings[nameof(TenantId)]?.Value;
            DeviceId = appSettingSection.Settings[nameof(DeviceId)]?.Value;
            LocationId = appSettingSection.Settings[nameof(LocationId)]?.Value;
            PrintFontFamily = appSettingSection.Settings[nameof(PrintFontFamily)]?.Value;
            PrinterName = appSettingSection.Settings[nameof(PrinterName)]?.Value;

            PrinterCodePage = appSettingSection.Settings[nameof(PrinterCodePage)]?.Value;
            PrinterCodePage = string.IsNullOrEmpty(PrinterCodePage) ? "857" : PrinterCodePage;

            PrinterCharPerLine = appSettingSection.Settings[nameof(PrinterCharPerLine)]?.Value;
            PrinterCharPerLine = string.IsNullOrEmpty(PrinterCharPerLine) ? "48" : PrinterCharPerLine;

            NetsPort = appSettingSection.Settings[nameof(NetsPort)]?.Value;
            OpenSchedulesShow = bool.Parse(appSettingSection.Settings[nameof(OpenSchedulesShow)]?.Value ?? "false");

            ShowLanguageSelection = bool.Parse(appSettingSection.Settings[nameof(ShowLanguageSelection)]?.Value ?? "false");
            ShowMemberButton = bool.Parse(appSettingSection.Settings[nameof(ShowMemberButton)]?.Value ?? "false");

            ShowAscentis = bool.Parse(appSettingSection.Settings[nameof(ShowAscentis)]?.Value ?? "false");

            BackgroundImage = appSettingSection.Settings[nameof(BackgroundImage)]?.Value;
            MenuBackgroundTransparency = double.Parse(appSettingSection.Settings[nameof(MenuBackgroundTransparency)]?.Value ?? "0.6");

            AscentisApiLink = appSettingSection.Settings[nameof(AscentisApiLink)]?.Value;
            AscentisCampaign = appSettingSection.Settings[nameof(AscentisCampaign)]?.Value;
            AscentisCashierIdentifier = appSettingSection.Settings[nameof(AscentisCashierIdentifier)]?.Value;
            AscentisDatabase = appSettingSection.Settings[nameof(AscentisDatabase)]?.Value;
            AscentisEnquiryCode = appSettingSection.Settings[nameof(AscentisEnquiryCode)]?.Value;
            AscentisMembershipType = appSettingSection.Settings[nameof(AscentisMembershipType)]?.Value;
            AscentisOutletCode = appSettingSection.Settings[nameof(AscentisOutletCode)]?.Value;
            AscentisPassword = appSettingSection.Settings[nameof(AscentisPassword)]?.Value;
            AscentisPosIdentifier = appSettingSection.Settings[nameof(AscentisPosIdentifier)]?.Value;
            AscentisUser = appSettingSection.Settings[nameof(AscentisUser)]?.Value;


            AppliedTerminal = appSettingSection.Settings[nameof(AppliedTerminal)]?.Value;

            CurrencySymbol = appSettingSection.Settings[nameof(CurrencySymbol)]?.Value;
            CurrencySymbol = string.IsNullOrEmpty(CurrencySymbol) ? "$" : CurrencySymbol;

        }

        public static void SaveSettings()
        {
            AddOrUpdateAppSettings(nameof(Url), Url);
            AddOrUpdateAppSettings(nameof(Tenant), Tenant);
            AddOrUpdateAppSettings(nameof(User), User);
            AddOrUpdateAppSettings(nameof(Password), Password);
            AddOrUpdateAppSettings(nameof(TenantId), TenantId);
            AddOrUpdateAppSettings(nameof(DeviceId), DeviceId);
            AddOrUpdateAppSettings(nameof(PrintFontFamily), PrintFontFamily);
            AddOrUpdateAppSettings(nameof(PrinterName), PrinterName);
            AddOrUpdateAppSettings(nameof(LocationId), LocationId);
            AddOrUpdateAppSettings(nameof(PrinterCodePage), PrinterCodePage);
            AddOrUpdateAppSettings(nameof(PrinterCharPerLine), PrinterCharPerLine);
            AddOrUpdateAppSettings(nameof(NetsPort), NetsPort);
            AddOrUpdateAppSettings(nameof(OpenSchedulesShow), OpenSchedulesShow.ToString());
            AddOrUpdateAppSettings(nameof(ShowLanguageSelection), ShowLanguageSelection.ToString());
            AddOrUpdateAppSettings(nameof(ShowMemberButton), ShowMemberButton.ToString());
            AddOrUpdateAppSettings(nameof(BackgroundImage), BackgroundImage);
            AddOrUpdateAppSettings(nameof(MenuBackgroundTransparency), MenuBackgroundTransparency.ToString());

            AddOrUpdateAppSettings(nameof(ShowAscentis), ShowAscentis.ToString());

            AddOrUpdateAppSettings(nameof(AscentisApiLink), AscentisApiLink);
            AddOrUpdateAppSettings(nameof(AscentisCampaign), AscentisCampaign);
            AddOrUpdateAppSettings(nameof(AscentisCashierIdentifier), AscentisCashierIdentifier);
            AddOrUpdateAppSettings(nameof(AscentisDatabase), AscentisDatabase);
            AddOrUpdateAppSettings(nameof(AscentisEnquiryCode), AscentisEnquiryCode);
            AddOrUpdateAppSettings(nameof(AscentisMembershipType), AscentisMembershipType);
            AddOrUpdateAppSettings(nameof(AscentisOutletCode), AscentisOutletCode);
            AddOrUpdateAppSettings(nameof(AscentisPassword), AscentisPassword);
            AddOrUpdateAppSettings(nameof(AscentisPosIdentifier), AscentisPosIdentifier);
            AddOrUpdateAppSettings(nameof(AscentisUser), AscentisUser);

            AddOrUpdateAppSettings(nameof(CurrencySymbol), CurrencySymbol);
            AddOrUpdateAppSettings(nameof(AppliedTerminal), AppliedTerminal);

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
