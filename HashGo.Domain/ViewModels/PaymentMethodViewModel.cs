using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Models;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using Microsoft.EntityFrameworkCore.Update.Internal;
using HashGo.Core.Models.BestTech;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Net;
using HashGo.Domain.Models.Base;
using System.Globalization;
using HashGo.Infrastructure.Events;
using Prism.Events;
using HashGo.Core.Contracts.View;
using HashGo.Domain.Services;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace HashGo.Domain.ViewModels
{
    public partial class PaymentMethodViewModel : BaseResturantViewModel
    {
        [ObservableProperty]
        IEnumerable<PaymentMethod> paymentMethods;

        [ObservableProperty]
        Cart cart;

        private INetworkService _NetworkService;
        IRetailConnectService retailConnectService;
        IEventAggregator eventAggregator;
        IPopupService popupService;
        SharedDataService sharedDataService;


        public PaymentMethodViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                      INavigationService navigationService,
                                      IOrderService orderService, INetworkService networkService,
                                      IRetailConnectService retailConnectService,
                                      IEventAggregator eventAggregator,
                                      IPopupService popupService, SharedDataService sharedDataService) 
            : base(loggingService, brandService, navigationService, orderService)
        {
            this.popupService = popupService;
            this.eventAggregator = eventAggregator;
            _NetworkService = networkService;
            this.retailConnectService = retailConnectService;
            this.sharedDataService = sharedDataService;
        }
        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.PaymentMethods = await this.OrderService.ReadPaymentMethod();

            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(LoadDataAsync)}() Completed.");

        }

        private bool CanProcessPaymentMethod(PaymentMethod paymentMethod) { return !this.IsLoading && paymentMethod != null; }

        [RelayCommand]
        void NavigateToPaymentPageScreen()
        {
            //var parameters = new Dictionary<string, object>
            //    {
            //        { "IsServiceDepartment", sharedDataService.DepartmentName.ToUpper() == "SERVICING" },
            //    };
            //this.NavigationService.NavigateToAsync(Pages.DineDateSelect.ToString(), parameters);
            
            
            this.NavigationService.NavigateToAsync(Pages.CustomerDetails.ToString());
        }

        [RelayCommand(CanExecute = nameof(CanProcessPaymentMethod))]
        private async Task ProcessPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                IsBusy = true;
                this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(ProcessPaymentMethod)}() Started.");

                //Prepare sales order object and send it
                decimal total = 0;
                List<SalesOrderDetail> lstSaleOrderDetails = new List<SalesOrderDetail>();

                foreach (var selectedUnit in SelectedUnits)
                {
                    SalesOrderDetail detail = new SalesOrderDetail();
                    detail.unitId = selectedUnit.UnitId;
                    detail.productId = selectedUnit.Id;
                    detail.price = Convert.ToDecimal(selectedUnit.UnitPrice);
                    detail.qty = selectedUnit.UnitCount;
                    detail.subTotal = detail.price * detail.qty;
                    total += detail.subTotal;
                    lstSaleOrderDetails.Add(detail);
                }

                SalesOrderRequest salesOrderRequest = new SalesOrderRequest();
                salesOrderRequest.salesOrderDetail = lstSaleOrderDetails;
                salesOrderRequest.salesOrder = new SalesOrder()
                {  
                    date = ApplicationStateContext.CustomerDate, //DateTime.SpecifyKind(ApplicationStateContext.CustomerDate, DateTimeKind.Utc),
                    name = ApplicationStateContext.CustomerDetailsObj?.Name,
                    referralCode = this.ReferralCode,
                    contactNo = ApplicationStateContext.CustomerDetailsObj?.ContactNumber,
                    address = ApplicationStateContext.CustomerDetailsObj?.UnitNo,
                    postalCode = Convert.ToString(ApplicationStateContext.CustomerDetailsObj?.PostalCode),
                    unitName = ApplicationStateContext.CustomerDetailsObj?.UnitNo,
                    floorNumber = ApplicationStateContext.CustomerDetailsObj?.FloorNo, 
                    locationId = 138,
                    netTotal = total,
                    //saleOrderDetails = lstSaleOrderDetails 
                };

                // TODO 
                //saleOrder.Date = null;
                TransactionDetails transactionDetails = await retailConnectService.CreateSalesOrderWithPayment(salesOrderRequest);
                IsBusy = false;

                #region Payment   //TODO

                if (paymentMethod.Name.ToUpper() == "NETS")
                {
                    this.NavigationService.NavigateToAsync(Pages.QRPayment.ToString());
                }
                else if(paymentMethod.Name.ToUpper() == "VISA")
                {
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }

                #endregion

                    //ApplicationStateContext.ClearData();

                    //if(transactionDetails != null && 
                    //   !string.IsNullOrEmpty(transactionDetails.transactionNo))
                    //{

                    //    #region SelectPaymentCommand

                    //    if(paymentMethod.Name.Equals("PlutusCard"))
                    //    {

                    //    }

                    //    #endregion

                    //    this.OrderQueueNumber = _NetworkService.GetQueueNumber();

                    //    var parameters = new Dictionary<string, object>
                    //    {
                    //        { nameof(this.OrderId), this.OrderId },
                    //        { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                    //        { nameof(this.OrderQueueNumber), this.OrderQueueNumber },
                    //        { "SelectedPaymentMethod", paymentMethod},
                    //        { "TransactionNumber", transactionDetails.transactionNo},
                    //    };

                    //    await this.NavigateToPage(Pages.OrderConfirmation, parameters);
                    //}
                    //else
                    //{
                    //    this.NavigateToPage(Pages.RestaurantStartup, null);
                    //}

                    this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(ProcessPaymentMethod)}() Completed.");
            }
            
            catch(Exception e)
            {
                IsBusy= false;
            }
        }

        //public async void SelectPaymentCommand(DineGoPaymentType? paymentType)
        //{
        //    try
        //    {
        //        if (FlyTicketResponse?.IsError == false && paymentType != null)
        //        {
        //            PaymentDesc = paymentType.DineGoDesc;
        //            if (paymentType.PaymentType.PaymentTenderType == 0)
        //            {
        //                if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName) &&
        //                    paymentType.PaymentType.ProcessorName.Equals("PlutusCard"))
        //                {
        //                    if (FlyTicketResponse.Response.TotalAmount < 1)
        //                    {
        //                        EventAggregator.Publish(
        //                            new MessageBoxEvent("Minimum Amount for the Transaction is Rs.1"));
        //                        return;
        //                    }

        //                    var paymentSuccess = false;
        //                    var status = string.Empty;

        //                    var worker = new BackgroundWorker();
        //                    var cardProcess = new PlutusCardProcess();
        //                    worker.DoWork += (s, e) =>
        //                    {
        //                        IsPlutusPayment = true;
        //                        while (_plutusPaymentType == 0) Thread.CurrentThread.Join(1000);
        //                        if (_plutusPaymentType != -1)
        //                        {
        //                            LoadingAnimationIsShow = true;
        //                            PlutusPaymentMessageIsShow = true;

        //                            if (_plutusPaymentType == 1)
        //                                PlutusPaymentMessage = "Please insert/tap your card with the payment terminal";
        //                            else if (_plutusPaymentType == 2)
        //                                PlutusPaymentMessage = "Scan QR on EDC";
        //                            else if (_plutusPaymentType == 3)
        //                                PlutusPaymentMessage = "Processing";

        //                            paymentSuccess = cardProcess.DoSale((decimal)FlyTicketResponse.Response.TotalAmount,
        //                                "GoTicket", _plutusPaymentType, ref status);
        //                        }
        //                    };
        //                    worker.RunWorkerCompleted += async (s, e) =>
        //                    {
        //                        IsPlutusPayment = false;
        //                        LoadingAnimationIsShow = false;
        //                        PlutusPaymentMessageIsShow = false;
        //                        PlutusPaymentMessage = "";
        //                        if (_plutusPaymentType != -1)
        //                        {
        //                            if (paymentSuccess)
        //                            {
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                var myTicketId = PushFlyTicketRequest();
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                if (myTicketId > 0)
        //                                {
        //                                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));


        //                                    var paymentPush = PushPlutusPaymentInformation(
        //                                        FlyTicketResponse.Response.Id,
        //                                        cardProcess.PaymentFrame, paymentType.PaymentType.Name);
        //                                    IsShowPaymentSuccess = true;
        //                                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                    if (paymentPush)
        //                                    {
        //                                        PaymentMessage =
        //                                            Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];

        //                                        var printContent = GetPrintContent(FlyTicketResponse.Response.Id);
        //                                        if (printContent != null && printContent.Any() &&
        //                                            AppSettings.PrinterName != null)
        //                                        {
        //                                            PrintService service = new();
        //                                            await service.PrintBytes(new PrinterSetting
        //                                            {
        //                                                ShareName = AppSettings.PrinterName,
        //                                                CharsPerLine = Convert.ToInt32(AppSettings.PrinterCharPerLine),
        //                                                CodePage = Convert.ToInt32(AppSettings.PrinterCodePage)
        //                                            }, printContent);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        EventAggregator.Publish(
        //                                            new MessageBoxEvent(
        //                                                Localizer.Instance["PaymentSuccessfulPaymentSlip"]));
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    status = Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];
        //                                    EventAggregator.Publish(new MessageBoxEvent(status));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                NLogger.Error(status);
        //                                EventAggregator.Publish(new MessageBoxEvent(status));
        //                            }
        //                        }

        //                        _plutusPaymentType = 0;
        //                    };
        //                    worker.RunWorkerAsync();
        //                }
        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName) &&
        //                         paymentType.PaymentType.ProcessorName.Equals("InputTronics"))
        //                {
        //                    NLogger.Error("Coming Inside InputTronics");


        //                    var paymentSuccess = false;
        //                    var status = string.Empty;
        //                    var worker = new BackgroundWorker();
        //                    var cardProcess = new InputTronicsProcess();
        //                    var appSettings = AppSettings.NetsPort;
        //                    var ipAddress = "";
        //                    var portNumber = 0;
        //                    if (!string.IsNullOrEmpty(appSettings) && appSettings.Contains("@"))
        //                    {
        //                        var splitAddress = appSettings.Split('@');
        //                        ipAddress = splitAddress[0];
        //                        portNumber = Convert.ToInt32(splitAddress[1]);
        //                    }



        //                    if (!string.IsNullOrEmpty(ipAddress) && portNumber > 0)
        //                    {
        //                        worker.DoWork += (s, e) =>
        //                        {
        //                            LoadingAnimationIsShow = true;
        //                            paymentSuccess = cardProcess.ProcessPayment(ipAddress, portNumber,
        //                                (decimal)FlyTicketResponse.Response.TotalAmount);
        //                        };
        //                        worker.RunWorkerCompleted += async (s, e) =>
        //                        {
        //                            LoadingAnimationIsShow = false;
        //                            if (paymentSuccess)
        //                            {
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                var myTicketId = PushFlyTicketRequest();
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                if (myTicketId > 0)
        //                                {
        //                                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                    var paymentPush = PushNETSPaymentInformation(FlyTicketResponse.Response.Id,
        //                                        cardProcess.Frame, paymentType?.PaymentType?.Name ?? "NETS");
        //                                    IsShowPaymentSuccess = true;
        //                                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                    if (paymentPush)
        //                                    {
        //                                        PaymentMessage =
        //                                            Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];

        //                                        var printContent = GetPrintContent(FlyTicketResponse.Response.Id);
        //                                        if (printContent != null && printContent.Any() &&
        //                                            AppSettings.PrinterName != null)
        //                                        {
        //                                            var service = new PrintService();
        //                                            await service.PrintBytes(new PrinterSetting
        //                                            {
        //                                                ShareName = AppSettings.PrinterName,
        //                                                CharsPerLine = Convert.ToInt32(AppSettings.PrinterCharPerLine),
        //                                                CodePage = Convert.ToInt32(AppSettings.PrinterCodePage)
        //                                            }, printContent);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        EventAggregator.Publish(
        //                                            new MessageBoxEvent(
        //                                                Localizer.Instance["PaymentSuccessfulPaymentSlip"]));
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    status = Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];
        //                                    EventAggregator.Publish(new MessageBoxEvent(status));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                NLogger.Error(status);
        //                                EventAggregator.Publish(new MessageBoxEvent(status));
        //                            }
        //                        };
        //                        worker.RunWorkerAsync();
        //                    }
        //                }

        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName) &&
        //                         paymentType.PaymentType.ProcessorName.Equals("KioskDemo"))
        //                {
        //                    var paymentSuccess = false;
        //                    var status = string.Empty;
        //                    var worker = new BackgroundWorker();
        //                    worker.DoWork += (s, e) => { paymentSuccess = true; };
        //                    worker.RunWorkerCompleted += async (s, e) =>
        //                    {
        //                        if (paymentSuccess)
        //                        {
        //                            EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                            var myTicketId = PushFlyTicketRequest();
        //                            EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                            if (myTicketId > 0)
        //                            {
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                var paymentPush = PushNETSPaymentInformation(FlyTicketResponse.Response.Id,
        //                                    new PaymentFrame
        //                                    {
        //                                        CardIssuerName = "Demo"
        //                                    }, paymentType.PaymentType.Name);
        //                                IsShowPaymentSuccess = true;
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                if (paymentPush)
        //                                {
        //                                    PaymentMessage =
        //                                        Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];

        //                                    var printContent = GetPrintContent(FlyTicketResponse.Response.Id);
        //                                    if (printContent != null && printContent.Any() &&
        //                                        AppSettings.PrinterName != null)
        //                                    {
        //                                        var service = new PrintService();
        //                                        await service.PrintBytes(new PrinterSetting
        //                                        {
        //                                            ShareName = AppSettings.PrinterName,
        //                                            CharsPerLine = Convert.ToInt32(AppSettings.PrinterCharPerLine),
        //                                            CodePage = Convert.ToInt32(AppSettings.PrinterCodePage)
        //                                        }, printContent);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    EventAggregator.Publish(
        //                                        new MessageBoxEvent(
        //                                            Localizer.Instance["PaymentSuccessfulPaymentSlip"]));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                status = Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];
        //                                EventAggregator.Publish(new MessageBoxEvent(status));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            NLogger.Error(status);
        //                            EventAggregator.Publish(new MessageBoxEvent(status));
        //                        }
        //                    };
        //                    worker.RunWorkerAsync();
        //                }
        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName)
        //                         && paymentType.PaymentType.ProcessorName.Equals("Stripe"))
        //                {
        //                    var processor =
        //                        JsonConvert.DeserializeObject<StripProcessor>(paymentType.PaymentType.Processors);
        //                    try
        //                    {
        //                        EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                        var productService = new ProductService();
        //                        var response = productService.GetStripePayNowQrCode(processor.SecretKey
        //                            , (decimal)FlyTicketResponse.Response.TotalAmount
        //                            , processor.Currency);
        //                        if (response != null)
        //                        {
        //                            var stripeResponse =
        //                                JsonConvert.DeserializeObject<BaseResponse<GetStripePayNowQrCodeResponse>>(
        //                                    response);
        //                            // Get stripe html
        //                            var asm = Assembly.GetExecutingAssembly();
        //                            var path = Path.GetDirectoryName(asm.Location);
        //                            var text = File.ReadAllText($"{path}/Assets/html/stripe_template.html");
        //                            text = text.Replace("{{publickey}}", processor.PublishKey);
        //                            text = text.Replace("{{clientSecret}}", stripeResponse.Result.ClientSecret);
        //                            var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
        //                            File.WriteAllText(tempFile, text);

        //                            IsStripeQrCodeShow = true;
        //                            StripeQrCodeHtmlFile = tempFile;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        IsStripeQrCodeShow = false;
        //                        NLogger.Error(ex);
        //                        EventAggregator.Publish(new MessageBoxEvent(ex.Message));
        //                    }
        //                    finally
        //                    {
        //                        EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));
        //                    }
        //                }
        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName)
        //                         && paymentType.PaymentType.ProcessorName.Equals("IPay88"))
        //                {
        //                    NLogger.Error("Coming Inside IPay88");
        //                    CurrentPaymentType = paymentType;
        //                    ShowCouponCommand();

        //                }
        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName)
        //                         && paymentType.PaymentType.ProcessorName.Equals("KeenuRetail"))
        //                {
        //                    NLogger.Error("Coming Inside KeenuRetail");
        //                    var keenuSettings = new KeenuProcessorSettings();
        //                    if (!string.IsNullOrEmpty(paymentType.PaymentType.Processors))
        //                    {
        //                        keenuSettings = JsonConvert.DeserializeObject<KeenuProcessorSettings>(paymentType.PaymentType.Processors);
        //                    }
        //                    var paymentSuccess = false;
        //                    var status = string.Empty;
        //                    var worker = new BackgroundWorker();
        //                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                    var myTicket = PushFlyTicketRequestObject();
        //                    EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));
        //                    worker.DoWork += (s, e) =>
        //                    {
        //                        if (myTicket != null && !string.IsNullOrEmpty(myTicket.TicketNumber))
        //                        {
        //                            var processKeenu = new KeenuProcess(keenuSettings.Port, GetParam(myTicket.TicketNumber, myTicket.TotalAmount), ref status);
        //                        }
        //                    };
        //                    worker.RunWorkerCompleted += async (s, e) =>
        //                    {
        //                        if (paymentSuccess)
        //                        {
        //                            if (myTicket != null && !string.IsNullOrEmpty(myTicket.TicketNumber))
        //                            {
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                var paymentPush = PushNETSPaymentInformation(FlyTicketResponse.Response.Id,
        //                                    new PaymentFrame(), paymentType.PaymentType.Name);
        //                                IsShowPaymentSuccess = true;
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                if (paymentPush)
        //                                {
        //                                    PaymentMessage =
        //                                        Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];

        //                                    var printContent = GetPrintContent(FlyTicketResponse.Response.Id);
        //                                    if (printContent != null && printContent.Any() &&
        //                                        AppSettings.PrinterName != null)
        //                                    {
        //                                        var service = new PrintService();
        //                                        await service.PrintBytes(new PrinterSetting
        //                                        {
        //                                            ShareName = AppSettings.PrinterName,
        //                                            CharsPerLine = Convert.ToInt32(AppSettings.PrinterCharPerLine),
        //                                            CodePage = Convert.ToInt32(AppSettings.PrinterCodePage)
        //                                        }, printContent);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    EventAggregator.Publish(
        //                                        new MessageBoxEvent(
        //                                            Localizer.Instance["PaymentSuccessfulPaymentSlip"]));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                status = Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];
        //                                EventAggregator.Publish(new MessageBoxEvent(status));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            NLogger.Error(status);
        //                            EventAggregator.Publish(new MessageBoxEvent(status));
        //                        }
        //                    };
        //                    worker.RunWorkerAsync();

        //                }
        //                else if (!string.IsNullOrEmpty(paymentType.PaymentType.ProcessorName)
        //                         && paymentType.PaymentType.ProcessorName.Equals("IPayOffline"))
        //                {
        //                    NLogger.Error("Coming Inside IPayOffline");
        //                    CurrentPaymentType = paymentType;
        //                    ScanIPayOfflineDoneCommand();
        //                }
        //                else
        //                {
        //                    var paymentSuccess = false;
        //                    var status = string.Empty;
        //                    var worker = new BackgroundWorker();
        //                    var cardProcess = new NetsProcess(AppSettings.NetsPort);
        //                    worker.DoWork += (s, e) =>
        //                    {
        //                        LoadingAnimationIsShow = true;
        //                        if (paymentType != null)
        //                            if (paymentType.PaymentType.Processor?.DefaultTag != null)
        //                                paymentSuccess = cardProcess.ProcessNets(
        //                                    paymentType?.PaymentType?.Processor?.DefaultTag,
        //                                    (decimal)FlyTicketResponse.Response.TotalAmount,
        //                                    "",
        //                                    out status);
        //                    };
        //                    worker.RunWorkerCompleted += async (s, e) =>
        //                    {
        //                        LoadingAnimationIsShow = false;
        //                        if (paymentSuccess)
        //                        {
        //                            EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                            var myTicketId = PushFlyTicketRequest();
        //                            EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                            if (myTicketId > 0)
        //                            {
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: true));
        //                                var paymentPush = PushNETSPaymentInformation(FlyTicketResponse.Response.Id,
        //                                    cardProcess.PaymentFrame, paymentType?.PaymentType?.Name ?? "NETS");
        //                                IsShowPaymentSuccess = true;
        //                                EventAggregator.Publish(new ViewPresenter(isShowLoadingAnimation: false));

        //                                if (paymentPush)
        //                                {
        //                                    PaymentMessage =
        //                                        Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];

        //                                    var printContent = GetPrintContent(FlyTicketResponse.Response.Id);
        //                                    if (printContent != null && printContent.Any() &&
        //                                        AppSettings.PrinterName != null)
        //                                    {
        //                                        var service = new PrintService();
        //                                        await service.PrintBytes(new PrinterSetting
        //                                        {
        //                                            ShareName = AppSettings.PrinterName,
        //                                            CharsPerLine = Convert.ToInt32(AppSettings.PrinterCharPerLine),
        //                                            CodePage = Convert.ToInt32(AppSettings.PrinterCodePage)
        //                                        }, printContent);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    EventAggregator.Publish(
        //                                        new MessageBoxEvent(
        //                                            Localizer.Instance["PaymentSuccessfulPaymentSlip"]));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                status = Localizer.Instance["PaymentIsSuccessfulPleaseProceedToCounter"];
        //                                EventAggregator.Publish(new MessageBoxEvent(status));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            NLogger.Error(status);
        //                            EventAggregator.Publish(new MessageBoxEvent(status));
        //                        }
        //                    };
        //                    worker.RunWorkerAsync();
        //                }

        //                await Task.Run(() => { Thread.Sleep(10000); });

        //                if (IsShowPaymentSuccess) StartOverCommand();
        //            }
        //            else
        //            {
        //                PaymentCashProcess();
        //            }
        //        }
        //        else
        //        {
        //            if (FlyTicketResponse?.Error.errorDescription != null)
        //            {
        //                NLogger.Error(FlyTicketResponse?.Error.errorDescription);
        //                EventAggregator.Publish(new MessageBoxEvent(FlyTicketResponse?.Error.errorDescription));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IsPlutusPayment = false;
        //        LoadingAnimationIsShow = false;
        //        PlutusPaymentMessageIsShow = false;
        //        PlutusPaymentMessage = "";
        //        IPayOfflinePaymentMessageIsShow = false;
        //        IPayOfflinePaymentMessage = "";
        //        _plutusPaymentType = 0;
        //        NLogger.Error(ex);
        //        EventAggregator.Publish(new MessageBoxEvent(ex.Message));
        //    }
        //}

        bool isBusy;
        public bool IsBusy 
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        [RelayCommand]
        void NavigateToStartScreen()
        {
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Publish(true);
            this.NavigateToPage(Pages.RestaurantStartup,null) ;
        }

        [RelayCommand]
        void NavigateToSelectProducts()
        {
            this.NavigateToPage(Pages.ProductSelection, null);
        }

        List<Unit> selectedUnits = new List<Unit>();

        public List<Unit> SelectedUnits
        {
            get => sharedDataService.SelectedUnits;
            set
            {
                sharedDataService.SelectedUnits = value;
                OnPropertyChanged();
            }
        }

        public string ReferralCode { get; private set; }
        
    }
}
