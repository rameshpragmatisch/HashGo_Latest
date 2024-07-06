using ControlzEx.Standard;
using HashGo.Core.Contracts.Enums;
using HashGo.Core.Contracts.View;
using HashGo.Core.Enum;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.Popups;
using HashGo.Wpf.App.BestTech.Views;
using HashGo.Wpf.App.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HashGo.Wpf.App.Services
{
    public class PopupService : IPopupService
    {
        TaskCompletionSource<bool> tcs;
        private readonly Dictionary<PopupType, Type> _popups = new Dictionary<PopupType, Type>();
        private readonly IServiceProvider _serviceProvider;

        public PopupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Configure(PopupType.eConfirmCustomerDetails, typeof(ConfirmCustomerDetailsPopup));
            Configure(PopupType.eResetOrder, typeof(ResetOrderPopup));
            Configure(PopupType.eRefferalCode, typeof(VirtualKeyboard));
        }

            public bool Configure(PopupType key, Type type)
            {
            lock (_popups)
            {
                if (_popups.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PopupService");
                }

                if (_popups.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_popups.First(p => p.Value == type).Key}");
                }

                _popups.Add(key, type);

                return true;
            }
        }

        public Type GetWindowType(PopupType popupType)
        {
            Type pageType;
            lock (_popups)
            {
                if (!_popups.TryGetValue(popupType, out pageType))
                {
                    throw new ArgumentException($"Popup not found: {popupType}. Did you forget to call PopupService.Configure?");
                }
            }

            return pageType;
        }

        Window GetWindow(PopupType popupType)
        {
            var pageType = GetWindowType(popupType);
            return _serviceProvider.GetService(pageType) as Window;
        }

        public bool ShowPopup(PopupType popupType)
        {
            Window window = GetWindow(popupType);
            window.Owner = Application.Current.MainWindow;
            bool? retValue = window.ShowDialog();

            //window.Closing += (s, e) =>
            //{
            //    e.Cancel = true;
            //    (s is Window window = Visibility.Hidden;

            //}

            return retValue.Value;
        }

        public async Task ShowPopupAsync(string message)
        {
            //tcs = new TaskCompletionSource<bool>();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(5);
            //MessagePopup messagePopup = new MessagePopup();
            //messagePopup.Topmost = true;
            //messagePopup.Message = message;
            //timer.Tick += (sender, e) =>
            //{
            //    timer.Stop();
            //    tcs.TrySetResult(true);
            //    messagePopup.Close();
            //};
            //timer.Start();

            //messagePopup.Show();
            //await tcs.Task;

            MessagePopup messagePopup = new MessagePopup();
            messagePopup.Topmost = true;
            messagePopup.Message = message;
            messagePopup.Show();
            await Task.Delay(3000);
            messagePopup.Close();
        }
    }
}
