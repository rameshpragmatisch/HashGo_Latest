using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HashGo.Core.Db
{
    public class QueueSettings : BaseEntity
    {
       
        public bool IsEnabled { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public int CurrentNumber { get; set; }
        public bool IsReset { get; set; }
        public DateTime LastUpdatedTime { get; set; } = DateTime.Now;

    }
}
