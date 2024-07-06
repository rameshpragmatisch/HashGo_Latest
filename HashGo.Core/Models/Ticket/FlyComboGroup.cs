using System.Collections.Generic;

namespace DinePlan.Common.Model.Point
{
    public class FlyCombo
    {
        public FlyCombo()
        {
            ComboGroups = new List<FlyComboGroup>();
        }

        public List<FlyComboGroup> ComboGroups { get; set; }
        public int MenuItemId { get; set; }
        public bool AddPrice { get; set; }
    }

    public class FlyComboGroup
    {
        public FlyComboGroup()
        {
            ComboItems = new List<FlyComboItem>();
        }

        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int Minimum { get; set; }

        public List<FlyComboItem> ComboItems { get; set; }
        public int Maximum { get; set; }
    }

    public class FlyComboItem
    {
        public FlyComboItem()
        {
            OrderTagGroups = new List<FlyOrderTagGroup>();
        }

        public string Name { get; set; }
        public string Files { get; set; }
        public int SortOrder { get; set; }
        public int MenuItemId { get; set; }
        public bool AutoSelect { get; set; }
        public decimal Price { get; set; }
        public int MaxQuantity { get; set; }
        public int Count { get; set; }
        public bool AddSeperately { get; set; }
        public IList<FlyOrderTagGroup> OrderTagGroups { get; set; }
        public string AliasName { get; set; }
    }
}