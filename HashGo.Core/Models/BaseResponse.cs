using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public BaseResponseError Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public BaseResponseError Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
    }
}
