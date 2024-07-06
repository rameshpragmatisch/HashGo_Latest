using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class Combo : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MenuItemId { get; set; }

        public bool IsShowCombo => ComboGroups?.Count > 0 ? true : false;

        private ObservableCollection<ComboGroup> _comboGroups;
        public ObservableCollection<ComboGroup> ComboGroups
        {
            get => _comboGroups;
            set
            {
                _comboGroups = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
