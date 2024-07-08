using HashGo.Core.Contracts.View;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for CustomerDetailsPage.xaml
    /// </summary>
    public partial class CustomerDetailsPage : BasePage
    {
        string programFiles = @"C:\Program Files\Common Files\Microsoft shared\ink";
        public CustomerDetailsPage(CustomerDetailsPageViewModel customerDetailsPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();

            this.DataContext = customerDetailsPageViewModel;

            this.Loaded += (sender, e) =>
            {
                tBoxName.Focus();


                try
                {
                    var uiHostNoLaunch = new UIHostNoLaunch();
                    var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                    tipInvocation.Toggle(GetDesktopWindow());
                    Marshal.ReleaseComObject(uiHostNoLaunch);
                }
                catch (Exception ex)
                {
                    string onScreenkeyboardPath = System.IO.Path.Combine(programFiles, "TabTip.exe");

                    ProcessStartInfo processStartInfo = new ProcessStartInfo(onScreenkeyboardPath);
                    processStartInfo.UseShellExecute = true;
                    Process oskProcess = Process.Start(processStartInfo);
                }   
            };

            this.Unloaded += (sender, e) => 
            {
                Process[] oskProcesses = Process.GetProcessesByName("TabTip");

                if (oskProcesses?.Length > 0)
                {
                    foreach (Process process in oskProcesses)
                    {
                        //process.Close();
                        process.Kill();
                    }
                }
            };
        }

        #region touch keyboard

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

        #endregion

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //e.Handled = true;
                MoveFocusToNextTextBox((UIElement)sender);
            }
        }

        void MoveFocusToNextTextBox(UIElement currentUIElement)
        {
            TraversalRequest traversalRequest = new TraversalRequest(FocusNavigationDirection.Next);
            UIElement nextElement = currentUIElement;

            do
            {
                nextElement.MoveFocus(traversalRequest);
                nextElement = Keyboard.FocusedElement as UIElement;
            } 
            while(nextElement is TextBlock);
        }
    }
}
