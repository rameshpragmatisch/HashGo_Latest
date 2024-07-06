using System.Windows.Controls;
using HashGo.Domain.ViewModels;

namespace HashGo.Wpf.App.Views.Pages;

/// <summary>
///     Interaction logic for ConnectCredentialsPage.xaml
/// </summary>
public partial class ConnectCredentialsPage : Page
{
    public ConnectCredentialsPage(ConnectCredentialsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}