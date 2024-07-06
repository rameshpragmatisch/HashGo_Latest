using CommunityToolkit.Mvvm.Input;
using HashGo.Domain.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class StoreLocatorsPopupViewModel : BaseViewModel
    {
        public StoreLocatorsPopupViewModel() 
        {
            CloseStoreLocatorsPopupCommand = new RelayCommand(OnCloseStoreLocatorsPopup);
        }

        void OnCloseStoreLocatorsPopup()
        {
            DiaglogResult = true;
        }

        #region Commands

        public ICommand CloseStoreLocatorsPopupCommand { get; private set; }

        #endregion

        #region Properties

        bool? diaglogResult;
        public bool? DiaglogResult
        {
            get => diaglogResult;
            set
            {
                diaglogResult = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
