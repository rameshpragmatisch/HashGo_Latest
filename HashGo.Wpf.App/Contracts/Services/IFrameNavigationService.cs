using HashGo.Core.Contracts.Views;
using HashGo.Wpf.App.Contracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Contracts.Services
{
    public interface IFrameNavigationService: INavigationService
    {
        event EventHandler<string> Navigated;
        void Initialize(IFrame shellFrame);
        void UnsubscribeNavigation();

        void CleanNavigation();
    }
}
