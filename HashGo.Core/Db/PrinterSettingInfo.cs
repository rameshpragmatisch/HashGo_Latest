using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Db
{
    public class PrinterSettingInfo : BaseEntity
    {

        public string PrinterStationName { get; set; }
        public int NumberOfCopies { get; set; }
        public string HeaderText { get; set; }
        public string FooterText { get; set; }

        public DateTime LastUpdatedTime { get; set; } = DateTime.Now;

    }
}
