using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Constants;
using HashGo.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class ImageNavigationControlViewModel : ObservableObject
    {
        #region Properties

        Unit selectedUnit;

        public Unit SelectedUnit { get => selectedUnit; set
            {
                selectedUnit = value;
            }
        }

        #endregion

        public ImageNavigationControlViewModel(Unit selectedUnit)
        {
            this.SelectedUnit = selectedUnit;

            ViewDetailsCommand = new RelayCommand<string>(OnViewDetails);
        }

        private void OnViewDetails(string selectedDetailText)
        {
            switch(selectedDetailText)
            {
                case HashGoUIConstants.Description:

                    break;

                case HashGoUIConstants.Specs:

                    break;
                case HashGoUIConstants.Warranty:

                    break;
                case HashGoUIConstants.Other:

                    break;
            }
        }

        #region ICommands

        public RelayCommand<string> ViewDetailsCommand { get; private set; }

        #endregion
    }
}
