using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class Response
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success { get; set; }
    }
}
