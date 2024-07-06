using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.Events
{
    public class ClearAllSelectedDataEvent : PubSubEvent<bool>
    {
        public bool IsClearAll { get; set; }
    }
}
