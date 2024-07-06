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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ConfirmPopup.xaml
    /// </summary>
    public partial class ConfirmPopup : Window
    {
        public ConfirmPopup(string message = "")
        {
            InitializeComponent();

            if(!string.IsNullOrEmpty(message))
            {
                tBlockMessage.Text = message;
            }
        }

        private void NoButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void YesButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
