using HashGo.Domain.Models.Base;
using HashGo.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace HashGo.Wpf.App.BestTech.Controls
{
    /// <summary>
    /// Interaction logic for VirtualKeyboard.xaml
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        SharedDataService sharedDataService;
        int processId = -1;
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
                //if (processId != -1)
                //{
                //    var oskProcess = Process.GetProcessById(processId);

                //    if (oskProcess != null)
                //    {
                //        oskProcess.Kill();
                //        processId = -1;
                //    }
                //}

                //uint WM_SYSCOMMAND = 0x0112;
                //UIntPtr SC_CLOSE = new UIntPtr(0xF060);
                //IntPtr y = new IntPtr(0);
                //IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                //PostMessage(KeyboardWnd, WM_SYSCOMMAND, SC_CLOSE, y);

                var uiHostNoLaunch = new UIHostNoLaunch();
                var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                tipInvocation.Toggle(IntPtr.Zero); // Pass IntPtr.Zero to close the keyboard
                Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch(Exception ex)
            {

            }
        }

        void OpenVirtualKeyboard()
        {
            try
            {
                var uiHostNoLaunch = new UIHostNoLaunch();
                var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                tipInvocation.Toggle(GetDesktopWindow());
                Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch(Exception ex)
            {

            }
            
        }

        //void OpenVirtualKeyboard()
        //{
        //    //ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe");
        //    //processStartInfo.UseShellExecute = true;

        //    //var oskProcess =  Process.Start(processStartInfo);
        //    //processId = oskProcess.Id;
        //    //oskProcess.Exited += (sender, e) =>
        //    //{
        //    //    processId = -1;
        //    //};

        //    //var uiHostNoLaunch = new UIHostNoLaunch();
        //    //var tipInvocation = (ITipInvocation)uiHostNoLaunch;
        //    //tipInvocation.Toggle(GetDesktopWindow());
        //    //Marshal.ReleaseComObject(uiHostNoLaunch);
        //    string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files\\Microsoft Shared\\ink\\TabTip.exe");
        //    if (!System.IO.File.Exists(path))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        ProcessStartInfo processStartInfo = new ProcessStartInfo
        //        {
        //            FileName = path,
        //            UseShellExecute = true
        //        };

        //        var oskProcess = Process.Start(processStartInfo);

        //        if (oskProcess != null)
        //        {
        //            processId = oskProcess.Id;
        //            oskProcess.EnableRaisingEvents = true;
        //            oskProcess.Exited += (sender, e) =>
        //            {
        //                processId = -1;
        //                //Console.WriteLine("On-Screen Keyboard has exited.");
        //            };

        //            //Console.WriteLine("Started On-Screen Keyboard with Process ID: " + processId);
        //        }
        //        else
        //        {
        //            //Console.WriteLine("Failed to start On-Screen Keyboard.");
        //        }
        //    }
        //    catch (COMException comEx)
        //    {
        //        //Console.WriteLine("COM exception occurred: " + comEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine("An error occurred while starting the On-Screen Keyboard: " + ex.Message);
        //    }
        //}

        #region Keyboard   TabTip.exe
        [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
        class UIHostNoLaunch
        {
        }

        [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ITipInvocation
        {
            void Toggle(IntPtr hwnd);
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] 
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //[DllImport("user32.dll", SetLastError = false)]
        //static extern IntPtr GetDesktopWindow();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string sClassName, string sAppName);

        #endregion

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
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
