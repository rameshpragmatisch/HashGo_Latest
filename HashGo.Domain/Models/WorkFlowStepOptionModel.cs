using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Contracts.Model;
using HashGo.Core.Models;

namespace HashGo.Domain.Models
{
    public partial class WorkFlowStepOptionModel : Base.GenericBaseModel<WorkFlowStepOption>, ISelectable
    {
        [ObservableProperty]
        private bool isSelected;

        public WorkFlowStepOptionModel(WorkFlowStepOption data) : base(data)
        {
        }
    }
}
