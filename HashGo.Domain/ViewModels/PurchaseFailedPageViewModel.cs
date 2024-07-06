using HashGo.Core.Contracts.Views;
using HashGo.Domain.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Domain.ViewModels
{
    public class PurchaseFailedPageViewModel : BaseViewModel
    {
        INavigationService navigationService;
        public PurchaseFailedPageViewModel(INavigationService navigationService) 
        {
            this.navigationService = navigationService;


        }

        #region Commands

        public ICommand NavigateToPreviousScreenCommand { get; private set; }
        public ICommand TryAgainCommand { get; private set; }

        #endregion
    }
}
