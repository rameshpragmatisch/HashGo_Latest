namespace HashGo.Core.Models
{
    public class AddOn : NameIdBase
    {
        public long MenuItemId { get; set; }
        public int Quantity { get; set; } = 0;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        //public WorkFlowStepOption[] StepOptions { get; set; } = Array.Empty<WorkFlowStepOption>();

        public WorkFlowStepOption? Value { get; set; } = null;
    }
}
