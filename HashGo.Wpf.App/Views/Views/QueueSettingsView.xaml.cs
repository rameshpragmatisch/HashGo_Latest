using HashGo.Core.Contracts.View;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Views.Views
{
    /// <summary>
    /// Interaction logic for QueueSettingsView.xaml
    /// </summary>
    public partial class QueueSettingsView : UserControl, IView, IHasDataContext
    {
        public QueueSettingsView()
        {
            InitializeComponent();
        }
    }
}
