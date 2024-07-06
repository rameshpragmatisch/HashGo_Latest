using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class WorkFlow : NameIdBase, INotifyPropertyChanged
    {
        public string Description { get; set; } = string.Empty;

        public long ResturantId { get; set; } = 0;

        public string ResturantUniqueId { get; set; } = string.Empty;

        public long MenuItemId { get; set; } = 0;


        private List<WorkFlowStep> _Steps;

        public List<WorkFlowStep> Steps
        {
            get
            {
                return _Steps;
            }
            set
            {
                _Steps = value;
                RaisePropertyChange("Steps");
            }
        }

        private ObservableCollection<TagWithQuantity> _selectedTagsForAllGroups;

        public ObservableCollection<TagWithQuantity> SelectedTagsForAllGroups
        {
            get
            {
                return _selectedTagsForAllGroups;
            }
            set
            {
                if (_selectedTagsForAllGroups is null) _selectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>();
                _selectedTagsForAllGroups = value;
                RaisePropertyChange("SelectedTagsForAllGroups");
            }
        }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion
    }
}
