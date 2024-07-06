using QRCoder.Xaml;
using QRCoder;
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
using HashGo.Wpf.App.BestTech.ViewModels;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Core.Contracts.View;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for QRPaymentPage.xaml
    /// </summary>
    public partial class QRPaymentPage : BasePage
    {
        public QRPaymentPage(QRPaymentPageViewModel qRPaymentPageViewModel, IPopupService popupService) : base(popupService)
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://rcsg.rtlconnect.net/", QRCodeGenerator.ECCLevel.H);
                XamlQRCode qrCode = new XamlQRCode(qrCodeData);
                DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20);
                qrIamge.Source = qrCodeAsXaml;
            };

            this.DataContext = qRPaymentPageViewModel;
        }
    }
}
