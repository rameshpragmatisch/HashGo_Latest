using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class WorkFlowStep : NameIdBase, INotifyPropertyChanged
    {
        public string Description { get; set; } = string.Empty;

        public bool IsOptional { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        public bool IsMultiSelectValue { get; set; }
        public string SubHeader { get; set; } = string.Empty;

        public WorkflowStepStatus Status { get; set; }

        public List<WorkFlowStepOption> Options { get; set; }

        public WorkFlowStepOption Value { get; set; }

        public List<WorkFlowStep> SubSteps { get; set; }

        private bool isMinimumQuantityChosen;

        public bool IsMinimumQuantityChosen
        {
            get
            {
                return isMinimumQuantityChosen;
            }
            set
            {
                isMinimumQuantityChosen = value;
                RaisePropertyChange("IsMinimumQuantityChosen");
            }
        }

        private ObservableCollection<TagWithQuantity> selectedTagsWithQuantities;

        public ObservableCollection<TagWithQuantity> SelectedTagsWithQuantities
        {
            get
            {
                return selectedTagsWithQuantities;
            }
            set
            {
                if (selectedTagsWithQuantities is null) selectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>();
                selectedTagsWithQuantities = value;
                RaisePropertyChange("SelectedTagsWithQuantities");
            }
        }

        private bool _IsActiveSelection;

        public bool IsActiveSelection
        {
            get
            {
                return _IsActiveSelection;
            }
            set
            {
                _IsActiveSelection = value;
                RaisePropertyChange("IsActiveSelection");
            }
        }

        public void RaiseSelectedTagsWithQuantities()
        {
            RaisePropertyChange("SelectedTagsWithQuantities");
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
