using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class MenuItemSchedule
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public string Days { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public int DayOfWeek
        {
            get => string.IsNullOrEmpty(Days) ? -1 : int.Parse(Days);
        }
    }
}
