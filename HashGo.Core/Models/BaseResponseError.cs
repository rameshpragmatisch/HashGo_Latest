using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class BaseResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string ValidationErrors { get; set; }
    }
}
