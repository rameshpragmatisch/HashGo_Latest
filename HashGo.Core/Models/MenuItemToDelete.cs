using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class MenuItemToDelete
    {
        public long Id { get; set; }
        public long MenuGroupId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Detail { get; set; }
        public string Icon { get; set; }

        public decimal Price { get; set; }

        public List<string> Tags { get; set; }

        public int Index { get; set; }

        public List<MenuItemExtra> MenuItemExtras { get; set; }

        public MenuItemType ItemType { get; set; } = MenuItemType.None;
    }
}
