using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class ScreenMenuResponse
    {
        public ScreenMenu[] Menu { get; set; }
    }

    public class ScreenMenu
    {
        public int Id { get; set; }
        public List<ScreenCategory> Categories { get; set; }
    }
}
