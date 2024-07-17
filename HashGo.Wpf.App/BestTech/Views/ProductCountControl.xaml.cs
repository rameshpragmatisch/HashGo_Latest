using HashGo.Wpf.App.Views.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ProductCountControl.xaml
    /// </summary>
    public partial class ProductCountControl : UserControl, INotifyPropertyChanged
    {
        public ProductCountControl()
        {
            InitializeComponent(); 
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public ICommand SubtractProductCommand
        {
            get { return (ICommand)GetValue(SubtractProductCommandProperty); }
            set { SetValue(SubtractProductCommandProperty, value); }
        }

        public static readonly DependencyProperty SubtractProductCommandProperty =
            DependencyProperty.Register("SubtractProductCommand", typeof(ICommand), typeof(ProductCountControl), new PropertyMetadata(null));

        public ICommand AddProductCommand
        {
            get { return (ICommand)GetValue(AddProductCommandProperty); }
            set { SetValue(AddProductCommandProperty, value); }
        }

        public static readonly DependencyProperty AddProductCommandProperty =
            DependencyProperty.Register("AddProductCommand", typeof(ICommand), typeof(ProductCountControl), new PropertyMetadata(null));

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(ProductCountControl), new PropertyMetadata(0));



        public object AddProductParameter
        {
            get { return (object)GetValue(AddProductParameterProperty); }
            set { SetValue(AddProductParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddProductParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddProductParameterProperty =
            DependencyProperty.Register("AddProductParameter", typeof(object), typeof(ProductCountControl), new PropertyMetadata(null));



        public object RemoveProductParamter
        {
            get { return (object)GetValue(RemoveProductParamterProperty); }
            set { SetValue(RemoveProductParamterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveProductParamter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveProductParamterProperty =
            DependencyProperty.Register("RemoveProductParamter", typeof(object), typeof(ProductCountControl), new PropertyMetadata(null));

        public bool CanMakeItemsCountZero { get; set; } = true;


        private void MinusControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Count == 0)
                return;

            if (!CanMakeItemsCountZero && Count == 1)
                return;

            Count--;

            SubtractProductCommand?.Execute(RemoveProductParamter);
        }

        private void PlusControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Count++;

            AddProductCommand?.Execute(AddProductParameter);
        }
    }
}
