using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HashGo.Wpf.App.Behavior
{
    public class TextBoxEnterNavigateBahaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.KeyDown += AssociatedObject_KeyDown;

            CommandManager.AddPreviewExecutedHandler(AssociatedObject, OnCommandExecuted);
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveFocusToNextTextBox((UIElement)sender);
                e.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            CommandManager.RemovePreviewExecutedHandler(AssociatedObject, OnCommandExecuted);
        }

        private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    //e.Handled = true;
                }
            }
        }

        void MoveFocusToNextTextBox(UIElement currentUIElement)
        {
            TraversalRequest traversalRequest = new TraversalRequest(FocusNavigationDirection.Next);
            UIElement nextElement = currentUIElement;

            do
            {
                nextElement.MoveFocus(traversalRequest);
                nextElement = Keyboard.FocusedElement as UIElement;
            }
            while (nextElement is TextBlock);
        }
    }
}
