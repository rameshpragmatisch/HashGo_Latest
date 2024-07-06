using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class QueueSetting
    {
        public bool EnableQueue { get; set; }

        public bool ResetQueue { get; set; }

        public string QueuePrefix { get; set; } = string.Empty;

        public string QueueSuffix { get; set; } = string.Empty;

        public int StartNumber { get; set; }

        public int EndNumber { get; set; }

    }
}
