using System.Security.Cryptography.Pkcs;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using HashGo.Wpf.App.Contracts.Services;
using HashGo.Wpf.App.Contracts.ViewModels;
using HashGo.Wpf.App.Contracts.Views;
using HashGo.Wpf.App.Helpers;

namespace HashGo.Wpf.App.Services
{

    public class NavigationService : IFrameNavigationService
    {
        private readonly IPageService _pageService;
        private IFrame _frame;
        private object _lastParameterUsed;
        private readonly Stack<object> navigationStack;

        public event EventHandler<string> Navigated;

        public bool CanNavigateToPreviousScreen => navigationStack.Count > 0;

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
            navigationStack = new Stack<object>();
        }

        public void Initialize(IFrame shellFrame)
        {
            if (_frame == null)
            {
                _frame = shellFrame;
            }
        }

        public void UnsubscribeNavigation()
        {
            _frame = null;
        }

        public async Task<bool> NavigateToPreviousScreen(object parameter = null)
        {
            if (CanNavigateToPreviousScreen)
            {
                var page = navigationStack.Pop();
                bool result = await Dispatcher.CurrentDispatcher.InvokeAsync<bool>(() =>
                {
                    if (page != null)
                    {
                        _frame.Content = page;

                        return true;
                    }

                    return false;
                });

                return result;
            }

            return false;
        }

        public async Task<bool> NavigateToAsync(string pageKey, object parameter = null, bool clearNavigation = false)
        {
            var pageType = _pageService.GetPageType(pageKey);

            if (_frame.Content?.GetType() != pageType || (parameter != null ))
            {
                //_frame.Tag = clearNavigation;
                var page = _pageService.GetPage(pageKey);
                if (page != null)
                {
                    var oldPage = _frame.Content;
                    bool result = await Dispatcher.CurrentDispatcher.InvokeAsync<bool>(() =>
                    {
                        var navigated = _frame.Navigate(page, parameter);
                        if (navigated)
                        {
                            _lastParameterUsed = parameter;
                            var dataContext = page.DataContext;

                            if (dataContext != null)
                            {
                                if (parameter is IDictionary<string, object> parameterDictionary)
                                {
                                    foreach (var item in parameterDictionary)
                                    {
                                        var currentValue = ObjectHelper.GetThePropertyValue(dataContext, item.Key);
                                        if (currentValue == null || !currentValue.Equals(double.NaN))
                                        {
                                            var success = ObjectHelper.SetThePropertyValue(dataContext, item.Key, item.Value);
                                        }
                                    }
                                }


                                if (dataContext is INavigationAware navigationAware)
                                {
                                    navigationAware.OnNavigatedFrom();
                                }
                            }

                            if (oldPage != null)
                            {
                                navigationStack.Push(oldPage);
                            }
                        }

                        return navigated;
                    });

                    return result;
                }
            }

            return false;
        }

        public async void CleanNavigation()
        {
            while (CanNavigateToPreviousScreen)
            {
                await this.NavigateToPreviousScreen();
            }
        }

        private void OnNavigated(IFrame frame, object extraData, bool clearNavigation)
        {
            //if (sender is IFrame frame)
            {
                //bool clearNavigation = (bool)frame.Tag;
                if (clearNavigation)
                {
                    this.CleanNavigation();
                }

                var dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(extraData);
                }

                Navigated?.Invoke(frame, dataContext.GetType().FullName);
            }
        }
    }
}