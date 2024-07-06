using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class ServiceUnit
    {
        public int id { get; set; }
        public string inventoryCode { get; set; }
        public string name { get; set; }
        public object productName { get; set; }
        public string aliasName { get; set; }
        public string unitName { get; set; }
        public int unitId { get; set; }
        public string imagePath { get; set; }
        public double price { get; set; }
        public string remarks { get; set; }
    }
}
