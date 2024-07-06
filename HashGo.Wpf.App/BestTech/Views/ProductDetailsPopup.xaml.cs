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
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ProductDetailsPopup.xaml
    /// </summary>
    public partial class ProductDetailsPopup : Window
    {
        public ProductDetailsPopup()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Border_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
