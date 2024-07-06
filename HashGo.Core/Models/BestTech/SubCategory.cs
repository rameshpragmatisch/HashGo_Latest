using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class SubCategory
    {
        public string name { get; set; }
        public string code { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public object lastModificationTime { get; set; }
        public object lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public int creatorUserId { get; set; }
        public int id { get; set; }
    }
}
