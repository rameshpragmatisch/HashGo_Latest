using HashGo.Domain.Models.Base;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Controls
{
    /// <summary>
    /// Interaction logic for VirtualKeyboard.xaml
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        SharedDataService sharedDataService;
        Process oskProcess;
        public VirtualKeyboard(SharedDataService sharedDataService)
        {
            InitializeComponent();
            this.sharedDataService = sharedDataService;

            this.Loaded += (sender, e) =>
            {
                tBoxInput.Focus();
                tBoxInput.Text = null;

                if(!string.IsNullOrEmpty(sharedDataService.RefferalCode))
                {
                    tBoxInput.Text = sharedDataService.RefferalCode;
                    tBoxInput.SelectionStart = sharedDataService.RefferalCode.Length;
                }

            };

            this.MinHeight = SystemParameters.PrimaryScreenHeight/4;
            this.Width = SystemParameters.PrimaryScreenWidth - 10;

            this.Unloaded += (sender, e) =>
            {
                closeVirtualKeyboard();
            };

            OpenVirtualKeyboard();
        }

        void closeVirtualKeyboard()
        {
            try
            {
                Process[] oskProcesses = Process.GetProcessesByName("TabTip");

                if(oskProcesses?.Length > 0)
                {
                    foreach (Process process in oskProcesses)
                    {
                        //process.Close();
                        process.Kill();
                    }
                }

                //var uiHostNoLaunch = new UIHostNoLaunch();
                //var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                //tipInvocation.Toggle(IntPtr.Zero); // Pass IntPtr.Zero to close the keyboard
                //Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch(Exception ex)
            {

            }
        }

        string programFiles = @"C:\Program Files\Common Files\Microsoft shared\ink";

        void OpenVirtualKeyboard()
        {
            try
            {
                string onScreenkeyboardPath = System.IO.Path.Combine(programFiles, "TabTip.exe");

                ProcessStartInfo processStartInfo = new ProcessStartInfo(onScreenkeyboardPath);
                processStartInfo.UseShellExecute = true;

               oskProcess = Process.Start(processStartInfo);

                //var uiHostNoLaunch = new UIHostNoLaunch();
                //var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                //tipInvocation.Toggle(GetDesktopWindow());
                //Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch (Exception ex)
            {

            }
            
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //e.Handled = true;
                closeVirtualKeyboard();
                sharedDataService.RefferalCode = tBoxInput.Text;
                this.DialogResult = true;
                return;
            }

            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                 (e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.CapsLock ||
                 e.Key == Key.Delete ||
                 e.Key == Key.Back))
            {
                e.Handled = true;
                return;
            }

        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(VirtualKeyboard), new PropertyMetadata(null));



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string key = btn.Content.ToString();
                SendKey(key);
            }
        }

        void SendKey(string key)
        {
            if (key == "Space Bar" || key == "Space")
            {
                key = " ";
            }
            else if (key == "Delete")
            {
                tBoxInput.Text = "";
                tBoxInput.Focus();
                tBoxInput.SelectionStart = 0;
                return;
            }
            else if (key == "Backspace")
            {
                tBoxInput.Text = tBoxInput.Text.Substring(0, tBoxInput.Text.Length - 1);
                tBoxInput.Focus();
                tBoxInput.SelectionStart = tBoxInput.Text.Length;
                return;
            }

            int selectionStart = tBoxInput.SelectionStart;
            tBoxInput.Text = tBoxInput.Text.Insert(selectionStart, key);
            tBoxInput.SelectionStart = selectionStart + key.Length;
            Text = tBoxInput.Text;
        }

        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sharedDataService.RefferalCode = tBoxInput.Text;
            //this.Close();
            this.DialogResult = true;
        }

        private void Path_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            closeVirtualKeyboard();
            //sharedDataService.RefferalCode = tBoxInput.Text;
            this.Close();
            //this.DialogResult = false;
        }

        private void tBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                closeVirtualKeyboard();
            }
        }
    }
}
