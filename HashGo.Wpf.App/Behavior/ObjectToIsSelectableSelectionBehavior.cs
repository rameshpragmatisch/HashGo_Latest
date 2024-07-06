using HashGo.Core.Contracts.Model;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HashGo.Wpf.App.Behavior
{
    public class ObjectToIsSelectableSelectionBehavior : Behavior<ButtonBase>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Click += AssociatedObject_Click;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            if (this.AssociatedObject.DataContext is ISelectable selectable)
            {
                selectable.IsSelected = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.Click += AssociatedObject_Click;
        }
    }
}
