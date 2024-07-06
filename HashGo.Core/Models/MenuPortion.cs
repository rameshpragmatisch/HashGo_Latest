using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class MenuPortion : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public int Multiplier { get; set; }
        public string Barcode { get; set; }
        public bool ChangePrice { get; set; }
        public SoldOutItem SoldOutItem { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsSoldOut
        {
            get
            {
                if (SoldOutItem != null)
                {
                    return SoldOutItem.Quantity <= 0;
                }
                return false;
            }
        }
        public int MenuItemId { get; set; }

        public void IsSoldOutTriggerChange()
        {
            OnPropertyChanged(nameof(IsSoldOut));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
