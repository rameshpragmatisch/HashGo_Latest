using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class DineGoDeviceDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdleTime { get; set; }
        public string PinCode { get; set; } = "7861";
        public DineGoBrand[] DineGoBrands { get; set; }
        public DineGoPaymentType[] DineGoPaymentTypes { get; set; }

        public string ThemeSettings { get; set; }

        public BasicThemeSetting? ThemeSettingsObj
        {
            get
            {
                if (!string.IsNullOrEmpty(ThemeSettings))
                {
                    return JsonConvert.DeserializeObject<BasicThemeSetting>(ThemeSettings);
                }

                return new BasicThemeSetting();
            }
        }

    }
}
