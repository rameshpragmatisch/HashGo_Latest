using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Model
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
