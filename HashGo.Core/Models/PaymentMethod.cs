using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class PaymentMethod : NameIdBase
    {
        public string Icon { get; set; }

        public string PaymentType { get; set; }
        public string Description { get; set; }
    }
}
