#region using


#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyMenu
    {
        public IList<FlyCategory> Categories { get; set; }
        public IList<FlyOrderTagGroup> OrderTagGroups { get; set; }
        public IList<FlyMenuItem> NonMenuItems { get; set; }
    }
    public class LocalizationStringDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        public int Id { get; set; }
        
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }
        
        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>
        ///     The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the culture.
        /// </summary>
        /// <value>
        ///     The culture.
        /// </value>
        public string Culture { get; set; }
    }

    public class FlyOrderTagGroup
    {
        public int Id { get; set; }
        public int MinSelectItems { get; set; }
        public int MaxSelectItems { get; set; }
        public IList<FlyOrderTags> OrderTags { get; set; }
        public bool AddPriceToOrder { get; set; }
        public bool FreeTagging { get; set; }

        public string UserString { get; set; }
        public string Prefix { get; set; }
    }

    public class FlyTicketTagGroup
    {
        public string Name { get; set; }
        public int DataType { get; set; }
        public List<string> Tags { get; set; }
    }

    public class FlyOrderTags
    {
        public string UserString { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int SortOrder { get; set; }
        public int MaxQuantity { get; set; }
    }

    public class FlyCategory
    {
        public FlyCategory()
        {
            OrderTagGroups = new List<FlyOrderTagGroup>();
        }

        public int Id { get; set; }
        public IList<FlyMenuItem> MenuItems { get; set; }
        public string UserString { get; set; }
        public int SortOrder { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool FastMenu { get; set; }

        public IList<FlyOrderTagGroup> OrderTagGroups { get; set; }
        public string LanguageDescriptions { get; set; }
    }

    public class FlyMenuItem
    {
        public FlyMenuItem()
        {
            OrderTagGroups = new List<FlyOrderTagGroup>();
        }

        public string Name { get; set; }
        public string AliasName { get; set; }
        public string UserString { get; set; }
        public string AliasCode { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsFavorite { get; set; }
        public IList<FlyMenuPortion> MenuPortions { get; set; }
        public IList<FlyMenuSchedule> Schedules { get; set; }
        public IList<FlyMenuUpItem> UpItems { get; set; }
        public int SortOrder { get; set; }
        public IList<FlyOrderTagGroup> OrderTagGroups { get; set; }
        public int ItemType { get; set; }
        public string SubTag { get; set; }
        public string LanguageDescriptions { get; set; }
    }

    public class FlyMenuUpItem
    {
        public int RefMenuItemId { get; set; }
        public decimal Price { get; set; }

        public bool AddBaseProductPrice { get; set; }
        public int ProductType { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public bool AddAuto { get; set; }
        public int AddQuantity { get; set; }
    }

    public class FlyMenuSchedule
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int StartMinute { get; set; }
        public int EndMinute { get; set; }
        public string Days { get; set; }
    }

    public class FlyMenuPortion
    {
        public int Id { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
    }
}