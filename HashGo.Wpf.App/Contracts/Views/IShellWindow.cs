namespace HashGo.Wpf.App.Contracts.Views
{
    public interface IShellWindow
    {
        IFrame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}