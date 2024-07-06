using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class MenuItemExtra
    {
        public long Id { get; set; }
        public long MenuItemId { get; set; }

        public string Name {  get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
