using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class ComboGroup : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        private ObservableCollection<ComboItem> _comboItems;
        public ObservableCollection<ComboItem> ComboItems
        {
            get => _comboItems;
            set
            {
                _comboItems = value;
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
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<ComboItem> ComboQueue = new List<ComboItem>();

        //public string QuantitySelectPromptText => $"{Name} ( {Localizer.Instance["Min"]} {Minimum}, {Localizer.Instance["Max"]} {Maximum} )";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
