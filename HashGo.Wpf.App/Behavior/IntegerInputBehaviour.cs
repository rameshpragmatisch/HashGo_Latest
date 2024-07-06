using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HashGo.Wpf.App.Behavior
{
    public class IntegerInputBehaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            CommandManager.AddPreviewExecutedHandler(AssociatedObject, OnCommandExecuted);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            CommandManager.RemovePreviewExecutedHandler(AssociatedObject, OnCommandExecuted);
        }

        private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Command ==  ApplicationCommands.Paste)
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    if (!IsTextAllowed(text))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Matches text that is not a number
            return !regex.IsMatch(text);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void AssociatedObject_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
           e.Handled =!IsTextAllowed(e.Text);
        }
    }
}
