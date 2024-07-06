using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Services;
using HashGo.Wpf.App.Views.Views;

namespace HashGo.Wpf.App.Services;

public class ViewService : IViewService
{
    private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;

    public ViewService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Configure(Core.Enum.Views.ProductMenu.ToString(), typeof(RestaurantMenuView));
        Configure(Core.Enum.Views.QueueSettings.ToString(), typeof(QueueSettingsView));

    }

    public Type GetViewType(string key)
    {
        Type pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    public IView GetView(string key)
    {
        var pageType = GetViewType(key);
        return _serviceProvider.GetService(pageType) as IView;
    }

    public bool Configure(string key, Type type)
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);

            return true;
        }
    }
}
