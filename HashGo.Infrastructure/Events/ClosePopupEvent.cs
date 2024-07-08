using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.Events
{
    public class ClosePopupEvent: PubSubEvent<bool>
    {
        public bool CanClosePopup { get; set; }
    }
}
