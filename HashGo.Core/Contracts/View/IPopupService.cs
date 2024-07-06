using HashGo.Core.Contracts.Enums;
using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.View
{
    public interface IPopupService
    {
        Task ShowPopupAsync(string message);
        bool ShowPopup(PopupType popupType);
        bool Configure(PopupType key, Type type);
    }
}
