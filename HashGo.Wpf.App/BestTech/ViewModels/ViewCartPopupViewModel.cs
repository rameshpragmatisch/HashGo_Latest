using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.Models.Base;
using HashGo.Wpf.App.BestTech.Views;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class ViewCartPopupViewModel :ObservableObject
    {
        readonly INavigationService navigationService;
        SharedDataService sharedDataService;

        public ViewCartPopupViewModel(SharedDataService sharedDataService,
                                      INavigationService navigationService)
        {
            this.sharedDataService = sharedDataService;
            this.navigationService = navigationService;
            //if (selectedUnits != null && selectedUnits.Count > 0)
            //{
            //    foreach(var unit in selectedUnits)
            //    {
            //        for (int i = 0; i < unit.UnitCount; i++)
            //            SelectedUnits.Add(unit);
            //    }
            //    SelectedUnits = new List<Unit>(SelectedUnits);
            //}
                
            NavigateToCheckoutScreen = new RelayCommand(OnNavigateToCheckoutScreen);
            EditProductCommand = new RelayCommand<Unit>(OnEditProduct);
            RemoveProductCommand = new RelayCommand<Unit>(OnRemoveProduct);
        }

        public List<Unit> SelectedUnitsToDisplay 
        {
            get 
            { 
                IsEnabled = sharedDataService.SelectedUnits?.Count > 0;
                return sharedDataService.SelectedUnits; 
            }
            set
            {
                sharedDataService.SelectedUnits = value;

                IsEnabled = false;
                if (value?.Count>0)
                {
                    IsEnabled = true;
                }

                OnPropertyChanged();
            }
        }

        private void OnRemoveProduct(Unit unit)
        {
            if (unit == null ||
                sharedDataService?.SelectedUnits == null ||
                sharedDataService?.SelectedUnits.Count == 0)
                return;

            sharedDataService.SelectedUnits.Remove(unit);
            SelectedUnitsToDisplay = new List<Unit>(sharedDataService.SelectedUnits);
            OnPropertyChanged(nameof(SelectedUnitsToDisplay));
        }

        private void OnEditProduct(Unit unit)
        {
            if(unit == null) return;

            DiaglogResult = true;
            sharedDataService.SelectedUnit = unit;
            var parameters = new Dictionary<string, object>
            {
                { "CanAddItem", false },
            };
            navigationService.NavigateToAsync(Pages.Addons.ToString(), parameters);

            
        }

        private void OnNavigateToCheckoutScreen()
        {
            this.DiaglogResult = true;
            navigationService.NavigateToAsync(Pages.Payment.ToString());   //parameters
        }

        #region Commands

        public ICommand NavigateToCheckoutScreen {  get; private set; }
        public RelayCommand<Unit> EditProductCommand { get; private set; }
        public RelayCommand<Unit> RemoveProductCommand { get; private set; }


        #endregion

        #region properties

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

        bool isEnabled = false;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
