using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HashGo.Wpf.App.Contracts.Views;
using HashGo.Wpf.App.ViewModels;
using MahApps.Metro.Controls;


namespace HashGo.Wpf.App.Views
{

    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        DispatcherTimer timer;
        private const long cIdleMinutes = 3;

        bool running = true;

        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;
            //IdleTimerInit();
        }

        private void IdleTimerInit()
        {
            InputManager.Current.PreProcessInput += Idle_PreProcessInput;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(cIdleMinutes),
                IsEnabled = false
            }; // = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void Idle_PreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            timer.IsEnabled = false;
            timer.IsEnabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.IsEnabled = false;
            MessageBox.Show("Event Raised");
        }

        public IFrame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}