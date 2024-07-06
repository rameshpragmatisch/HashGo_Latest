using HashGo.Wpf.App.BestTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ViewCartPopup.xaml
    /// </summary>
    public partial class ViewCartPopup : Window
    {
        public double TargetTop { get; set; }
        public double TargetLeft { get; set; }

        Point cartImagePosition;
        bool isClosing = false;

        public ViewCartPopup(Point point)
        {
            InitializeComponent();

            cartImagePosition= point;

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            isClosing = true; // Set the flag to true when the window is closing
            if (Owner != null)
            {
                Owner.IsEnabled = true;
            }
        }

        // Event handler to handle window deactivation
        private void popupWindow_Deactivated(object sender, EventArgs e)
        {
            // Check if the window is closing before attempting to close it
            if (!isClosing)
            {
                Close();
            }
        }

        private void TextBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isClosing)
            {
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Point location = Mouse.GetPosition(this);
            //double from = SystemParameters.PrimaryScreenHeight;
            //double to = TargetTop;
            this.Top = SystemParameters.WorkArea.Bottom;
            this.Height = 477;

            var animation = new DoubleAnimation
            {
                From = SystemParameters.WorkArea.Bottom,   //
                To = SystemParameters.WorkArea.Bottom - this.Height,
                //From = 1536,   //
                //To = 1536 - 477,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            //this.Top = location.X-355;
            //this.Left = 0;  // location.Y;
            //this.Top = cartImagePosition.X;
            //this.Left = 0;  // cartImagePosition.Y;
            //this.Height = 477;
            this.Left = SystemParameters.WorkArea.Left+2;
            this.Width = SystemParameters.PrimaryScreenWidth-4;
            this.BeginAnimation(Window.TopProperty, animation);
            
        }

        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult= true;
        }
    }

    public static class DialogResultHelper
    {
        public static readonly DependencyProperty DialogResultProperty =
           DependencyProperty.RegisterAttached(
               "DialogResult",
               typeof(bool?),
               typeof(DialogResultHelper),
               new PropertyMetadata(null, OnDialogResultChanged));

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        public static bool? GetDialogResult(Window target)
        {
            return (bool?)target.GetValue(DialogResultProperty);
        }
        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                //window.DialogResult = (bool?)e.NewValue;
                window.Close();
            }
        }
    }
}
