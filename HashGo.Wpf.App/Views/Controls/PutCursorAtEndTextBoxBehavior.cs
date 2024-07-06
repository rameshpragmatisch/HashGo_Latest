using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace HashGo.Wpf.App.Views.Controls
{
    public class PutCursorAtEndTextBoxBehavior : Behavior<UIElement>
    {
        private TextBox _textBox;

        protected override void OnAttached()
        {
            base.OnAttached();

            _textBox = AssociatedObject as TextBox;

            if (_textBox == null)
                return;

            _textBox.TextChanged += textBox_TextChanged;
            ;
        }

        protected override void OnDetaching()
        {
            if (_textBox == null)
                return;

            _textBox.TextChanged -= textBox_TextChanged;

            base.OnDetaching();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _textBox.CaretIndex = _textBox.Text.Length;
        }
    }
}
