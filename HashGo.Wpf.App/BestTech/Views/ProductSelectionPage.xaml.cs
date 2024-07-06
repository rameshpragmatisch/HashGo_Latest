using HashGo.Core.Contracts.View;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for ProductSelectionPage.xaml
    /// </summary>
    public partial class ProductSelectionPage : BasePage,INotifyPropertyChanged
    {
        private DateTime _lastScroll;

        public ProductSelectionPage(ProductSelectionPageViewModel productSelectionPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();
            this.DataContext = productSelectionPageViewModel;
        }

        public bool IsScrolling
        {
            get { return !(DateTime.Now > _lastScroll.AddMilliseconds(100)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void lstBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _lastScroll = DateTime.Now;
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Thread.Sleep(100);
                OnPropertyChanged("IsScrolling");
            });
        }
    }
}
