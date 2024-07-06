using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.View
{
    public interface IHasDataContext
    {
        object DataContext { get; set; }
    }
}
