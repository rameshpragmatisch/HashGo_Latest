using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.Controls
{
    public class MouseClickBehaviour
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseClickBehaviour), new PropertyMetadata(OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is UIElement element)
            {
                if(e.NewValue != null)
                {
                    element.MouseLeftButtonDown += OnMouseLeftButtonDown;
                }
                else
                {
                    element.MouseLeftButtonDown -= OnMouseLeftButtonDown;
                }
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is UIElement element)
            {
                var command = GetCommand(element);

                if(command != null)
                {
                    var position = e.GetPosition(element);
                    if (command.CanExecute(position))
                    {
                        command.Execute(position);
                    }
                }
            }
        }
    }
}
