using HashGo.Core.Contracts.View;
using HashGo.Infrastructure.DataContext;
using HashGo.Wpf.App.BestTech.Controls;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ConfirmDineDatePage.xaml
    /// </summary>
    public partial class ConfirmDineDatePage : BasePage
    {
        public ConfirmDineDatePage(ConfirmDineDatePageViewModel confirmDineDatePageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();

            this.DataContext = confirmDineDatePageViewModel;
            

            
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Calendar cdr)
            {
                cdr.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
            }
            
        }

        private void calender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
