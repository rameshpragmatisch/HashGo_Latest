using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class CategorySchedule
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string StartMinute { get; set; }
        public string EndMinute { get; set; }
        public List<DisplayValue> AllDays { get; set; }
        public List<DisplayValue> AllMonthDays { get; set; }

        public DateTime Start { get => DateTime.Parse(StartDate); }

        public DateTime End { get => DateTime.Parse($"{EndDate} 23:59:59"); }
    }

    public class DisplayValue
    {
        public string Value { get; set; }
        public string DisplayText { get; set; }
        public bool IsSelected { get; set; }

        public int ValueOfInt { get => int.Parse(Value); }
    }
}
