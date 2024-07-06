using HashGo.Core.Contracts.View;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Wpf.App.BestTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public CustomerDetailsPage(CustomerDetailsPageViewModel customerDetailsPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();

            this.DataContext = customerDetailsPageViewModel;
        }

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
