using System.Windows;
using System.Windows.Controls;


namespace HashGo.Wpf.App.Selectors
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SpecialMenuItemTemplate { get; set; }

        public DataTemplate DetailMenuItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = this.DetailMenuItemTemplate;

            var menuItem = item as Core.Models.MenuItem;
            if (menuItem == null) return selectedTemplate;

            //switch (menuItem.ItemType)
            //{
            //    case Core.Enum.MenuItemType.Special:
            //        selectedTemplate = this.SpecialMenuItemTemplate;
            //        break;

            //    case Core.Enum.MenuItemType.Detail:
            //        selectedTemplate = this.DetailMenuItemTemplate;
            //        break;
            //}

            return selectedTemplate;
        }
    }
}
