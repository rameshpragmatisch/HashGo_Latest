using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class UpMenuItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public bool AddBaseProductPrice { get; set; }
        public int RefMenuItemId { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaxQty { get; set; }
        public bool AddAuto { get; set; }
        public int AddQuantity { get; set; }
        public int ProductType { get; set; }

        private MenuItem _menuItem;
        public MenuItem MenuItem
        {
            get => _menuItem;
            set
            {
                _menuItem = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (!value)
                {
                    ChooseQuantity = 1;
                }
                OnPropertyChanged();
            }
        }

        public UpMenuItem()
        {
            ChooseQuantity = MinimumQuantity > 0 ? MinimumQuantity : 1;
        }

        private int _chooseQuantity;
        public int ChooseQuantity
        {
            get => _chooseQuantity;
            set
            {
                _chooseQuantity = value < 1 ? 1 : value;
                OnPropertyChanged();
            }
        }

        public void AddQuantityCommand()
        {
            if ((ChooseQuantity < MaxQty && MaxQty != 0) || MaxQty == 0)
            {
                ChooseQuantity++;
            }
        }

        public void SubtractQuantityCommand()
        {
            if ((MinimumQuantity > 0 && ChooseQuantity - 1 >= MinimumQuantity) || MinimumQuantity == 0)
            {
                if (ChooseQuantity > 1)
                {
                    ChooseQuantity--;
                }
                else
                {
                    IsSelected = false;
                }
            }
        }

        public void CloseComboPopupCommand()
        {
            IsExpandedCombo = false;
        }

        private bool _isExpandedCombo;
        public bool IsExpandedCombo
        {
            get => _isExpandedCombo;
            set
            {
                _isExpandedCombo = value;
                OnPropertyChanged();
            }
        }

        public void UpSellingCommand()
        {
            if (MenuItem?.Combo?.ComboGroups?.Count > 0)
            {
                IsExpandedCombo = true;
            }

            if (IsSelected)
            {
                AddQuantityCommand();
            }
            IsSelected = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
