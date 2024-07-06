using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class PaymentType
    {
        public string Name { get; set; }
        public string AccountCode { get; set; }
        public bool AcceptChange { get; set; }
        public bool NoRefund { get; set; }
        public bool AutoPayment { get; set; }
        public bool DisplayInShift { get; set; }
        public int SortOrder { get; set; }
        public int PaymentProcessor { get; set; }
        public string ProcessorName { get; set; }
        public string Processors { get; set; }
        public string ButtonColor { get; set; }
        public string Files { get; set; }

        public int PaymentTenderType { get; set; }


        public int Id { get; set; }

        public Processors? Processor
        {
            get
            {
                if (!string.IsNullOrEmpty(Processors))
                {
                    return JsonConvert.DeserializeObject<Processors>(Processors);
                }
                return null;
            }
        }
    }
}
