using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class DineGoPaymentType
    {
        public string Label { get; set; }
        public PaymentType PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public string DineGoDesc { get; set; }
    }
}
