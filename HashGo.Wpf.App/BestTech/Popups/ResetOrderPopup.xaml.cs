using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Infrastructure.DataContext;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.Popups
{
    /// <summary>
    /// Interaction logic for ResetOrderPopup.xaml
    /// </summary>
    public partial class ResetOrderPopup : Window,INotifyPropertyChanged
    {
        DispatcherTimer timer;
        INavigationService navigationService;

        public ResetOrderPopup(INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = this;
            this.navigationService = navigationService;

            this.Loaded += (sender, e) =>
            {
                this.Height = SystemParameters.PrimaryScreenHeight / 2;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                timer = new DispatcherTimer()
                {
                     Interval = TimeSpan.FromSeconds(1)
                };
                
                timer.Tick += (sender, e) =>
                {
                    if (Count == 0)
                    {
                        timer.Stop();
                        timer = null;
                        this.Close();
                        navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
                    } 

                    Count--;
                };

                timer.Start();
            };
        }

        int count = 10;

        public int Count 
        {
            get => count;
            set
            {
                count = value;
                RaisePropertyChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChange([CallerMemberName]string propertyname="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer = null;
            this.DialogResult = true;
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                timer.Stop();
                timer = null;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
