using System.Collections.Generic;

namespace DinePlan.Common.Model.Point
{
    public class FlyComboResponse
    {
        public List<FlyCombo> ComboItems { get; set; }
        public int TotalCount { get; set; }
        public int SkipCount { get; set; }
        public int RecordCount { get; set; }


    }
}
