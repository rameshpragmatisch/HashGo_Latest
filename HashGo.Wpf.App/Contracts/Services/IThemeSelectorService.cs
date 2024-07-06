using DineGo.Wpf.App.Models;

namespace DineGo.Wpf.App.Contracts.Services;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
