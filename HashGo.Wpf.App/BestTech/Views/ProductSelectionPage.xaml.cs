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
        private bool _isTouchScrolling;
        private Point _touchStartPoint;

        private bool _isSubCategoriesTouchScrolling;
        private Point _subCategoriesTouchStartPoint;

        public ProductSelectionPage(ProductSelectionPageViewModel productSelectionPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();
            this.DataContext = productSelectionPageViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListBox_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            _touchStartPoint = e.GetTouchPoint(lstBox).Position;
            _isTouchScrolling = false;
        }

        private void ListBox_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (!_isTouchScrolling)
            {
                var touchCurrentPoint = e.GetTouchPoint(lstBox).Position;
                var diff = touchCurrentPoint - _touchStartPoint;

                if (Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    _isTouchScrolling = true;
                }
            }

            if (_isTouchScrolling)
            {
                e.Handled = false;
            }
        }

        private void ListBox_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (!_isTouchScrolling)
            {
                var item = VisualTreeHelper.HitTest(lstBox, e.GetTouchPoint(lstBox).Position)?.VisualHit;
                while (item != null && !(item is ListBoxItem))
                {
                    item = VisualTreeHelper.GetParent(item);
                }

                if (item is ListBoxItem listBoxItem)
                {
                    listBoxItem.IsSelected = true;
                }
            }
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isTouchScrolling)
            {
                e.Handled = true;
            }
        }

        #region SubCategories Touch

        private void LstBoxSubCategoreis_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            _subCategoriesTouchStartPoint = e.GetTouchPoint(lstBox).Position;
            _isSubCategoriesTouchScrolling = false;
        }

        private void LstBoxSubCategoreis_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (!_isSubCategoriesTouchScrolling)
            {
                var touchCurrentPoint = e.GetTouchPoint(lstBox).Position;
                var diff = touchCurrentPoint - _subCategoriesTouchStartPoint;

                if (Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    _isSubCategoriesTouchScrolling = true;
                }
            }

            if (_isSubCategoriesTouchScrolling)
            {
                e.Handled = false;
            }
        }

        private void LstBoxSubCategoreis_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (!_isSubCategoriesTouchScrolling)
            {
                var item = VisualTreeHelper.HitTest(lstBox, e.GetTouchPoint(lstBox).Position)?.VisualHit;
                while (item != null && !(item is ListBoxItem))
                {
                    item = VisualTreeHelper.GetParent(item);
                }

                if (item is ListBoxItem listBoxItem)
                {
                    listBoxItem.IsSelected = true;
                }
            }
        }

        private void LstBoxSubCategoreis_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isSubCategoriesTouchScrolling)
            {
                e.Handled = true;
            }
        }

        #endregion



    }
}
