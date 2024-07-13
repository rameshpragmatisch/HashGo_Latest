using HashGo.Core.Contracts.View;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.ApplicationModel.UserActivities;

namespace HashGo.Wpf.App.BestTech.Controls
{
    public class BasePage :Page
    {
        DispatcherTimer idleTimer;
        IPopupService popupService;

        public BasePage(IPopupService popupService)
        {
            this.Loaded += BasePage_Loaded;
            this.Unloaded += BasePage_Unloaded;
            this.popupService = popupService;
        }

        public BasePage()
        {
            this.Loaded += BasePage_Loaded;
            this.Unloaded += BasePage_Unloaded;
        }

        private void BasePage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            stopIdleTimer();
            idleTimer = null;

            this.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(userActivity));
            this.RemoveHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(userActivity));
            this.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown));
            this.RemoveHandler(UIElement.TouchDownEvent, new EventHandler<TouchEventArgs>(OnTouchDown));
            this.RemoveHandler(UIElement.MouseWheelEvent, new MouseWheelEventHandler(userActivity));
            this.RemoveHandler(UIElement.MouseMoveEvent, new MouseEventHandler(userActivity));

            if (DataContext is IBaseViewModel viewModel)
            {
                viewModel.ViewUnloaded();
            }
        }

        private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           if(DataContext is IBaseViewModel viewModel)
            {
                viewModel.ViewLoaded();

                if(popupService != null)
                {
                    idleTimer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(ApplicationStateContext.IdleTimeOutInSecs)
                    };

                    idleTimer.Tick += IdleTimer_Tick;

                    this.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(userActivity));
                    this.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(userActivity));
                    this.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown));
                    this.AddHandler(UIElement.TouchDownEvent, new EventHandler<TouchEventArgs>(OnTouchDown));
                    this.AddHandler(UIElement.MouseWheelEvent, new MouseWheelEventHandler(userActivity));
                    this.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(userActivity));
                    this.AddHandler(UIElement.TouchMoveEvent, new EventHandler<TouchEventArgs>(OnTouchDown));
                    this.AddHandler(UIElement.TouchUpEvent, new EventHandler<TouchEventArgs>(OnTouchDown));

                    startIdleTimer();
                }
                
            }
        }

        private void OnTouchDown(object sender, TouchEventArgs e)
        {
            restartIdleTimer();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            restartIdleTimer();
        }

        private void IdleTimer_Tick(object sender, EventArgs e)
        {
            popupService?.ShowPopup(Core.Contracts.Enums.PopupType.eResetOrder);
        }

        void startIdleTimer()
        {
            idleTimer?.Start();
        }

        void stopIdleTimer()
        {
            idleTimer?.Stop();
        }

        void restartIdleTimer()
        {
            idleTimer?.Stop();
            idleTimer?.Start();
        }

        void userActivity(object sender, EventArgs e)
        {
            restartIdleTimer();
        }
    }
}
