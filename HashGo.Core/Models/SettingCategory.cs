using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class SettingCategory : NameIdBase
    {
        public string Description { get; set; } = string.Empty;
        public string HelperText { get; set; } = string.Empty;

        public Enum.Views View { get; set; }
    }
}
