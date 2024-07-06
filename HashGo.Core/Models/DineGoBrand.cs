using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HashGo.Core.Models
{
    public class DineGoBrand
    {
        public string Label { get; set; }
        public int LocationId { get; set; }
        public DineGoDepartment[] DineGoDepartments { get; set; }
        public int ScreenMenuId { get; set; }
        public string OrderLink { get; set; }
        public string ThemeSettings { get; set; }

        public DineGoBrandThemeSetting? ThemeSettingObj
        {
            get
            {
                if (!string.IsNullOrEmpty(ThemeSettings))
                {
                    return JsonConvert.DeserializeObject<DineGoBrandThemeSetting>(ThemeSettings);
                }

                return new DineGoBrandThemeSetting();
            }
        }

        public DineGoBrand()
        {

        }

    }
   

    public class DineGoBrandThemeSetting
    { 
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public string BrandColor { get; set; }
        public string BrandImage { get; set; }
        public string CheckoutLogoImage { get; set; }
        public string HomeLogoImage { get; set; }
        public object NoProductsMessage { get; set; }
        public object PaymentSuccessMessage { get; set; }
        public string PrimaryButtonColor { get; set; }
        public string SecondaryButtonColor { get; set; }
        public string StartImage { get; set; }
        public string StartupBrandBgColor { get; set; }
        public string StartupBrandStripColor { get; set; }
        public object WelcomeMessage { get; set; }
    }




    public class BasicThemeSetting
    {
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public string BannerContents { get; set; }
        public string IdleContents { get; set; }
        public string Logo { get; set; }
        public object NoProductsMessage { get; set; }
        public object PaymentSuccessMessage { get; set; }
        public string PrimaryButtonColor { get; set; }
        public string SecondaryButtonColor { get; set; }
        public object WelcomeMessage { get; set; }

        public FilesInfo LogoObj { get;}
    }

    public class FileInfo
    {
        public string fileTag { get; set; }
        public string fileName { get; set; }
        public string fileSystemName { get; set; }
    }

    public class FilesInfo
    {
        public List<FileInfo> Collection { get; set; }
    }
}
