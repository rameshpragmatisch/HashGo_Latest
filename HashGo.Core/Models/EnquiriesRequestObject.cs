using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class EnquiriesRequestObject
    {
        public EnquiryRequest enquiry { get; set; }
    }

    public class EnquiryRequest
    {
        public string name { get; set; }
        public string message { get; set; }
        public string phoneNo { get; set; }
    }
}
