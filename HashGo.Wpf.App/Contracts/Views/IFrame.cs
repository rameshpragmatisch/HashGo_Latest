using System.Windows.Navigation;

namespace HashGo.Wpf.App.Contracts.Views
{
    public interface IFrame
    {
        //event NavigatedEventHandler Navigated;

        object Content { get; set; }

        //object Tag { get; set; }

        object GetDataContext();
        //void CleanNavigation();

        //bool Navigate(object content);

        bool Navigate(object content, object extraData);
    }
}
