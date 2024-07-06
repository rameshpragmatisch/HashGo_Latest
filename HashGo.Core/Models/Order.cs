using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class Order
    {
        public Order()
        {
            this.Cart = new Cart();
        }

        public long Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty ;

        public DiningOption DiningOption { get; set; } = DiningOption.None;

        public long CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
