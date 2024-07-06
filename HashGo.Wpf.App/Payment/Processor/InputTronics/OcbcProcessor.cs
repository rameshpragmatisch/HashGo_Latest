using System.ComponentModel;
using System.Text;
using DinePlan.Modules.PaymentModule.PaymentProcessors.InputTronics;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Models;
using HashGo.Core.Models.Ticket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HashGo.Wpf.App.Payment.Processor.InputTronics
{
    public class OcbcProcessor : IPaymentProcessor
    {
       
        private OcbcProcess _cardProcess;
        private readonly OcbcProcessorSettings _settings;
        private readonly ILoggingService _loggingService;

        private OcbcProcessor(OcbcProcessorSettings settings, ILoggingService loggingService)
        {
            _settings = settings;
            _loggingService = loggingService;
        }

       
        public decimal Preprocess(FlyTicket ticket, PaymentType paymentType, decimal dueAmount, decimal tenderedAmount)
        {
            return dueAmount;
        }

        public PaymentProcessResult Process(FlyTicket ticket, PaymentType paymentType, PaymentProcessResult processResult)
        {
            _cardProcess = new OcbcProcess(_settings, "", _loggingService,"");

            var dueAmount = processResult.DueAmount;
            var tenderedAmount = processResult.TenderedAmount;

           

            var paymentProcessResult = new PaymentProcessResult(dueAmount, tenderedAmount, processResult.Description);

            if (_settings.MaxAmount > new decimal(0) &&
                tenderedAmount > _settings.MaxAmount)
            {
              
                paymentProcessResult.TenderedAmount = _settings.MaxAmount;
                paymentProcessResult.CanContinueProcessing = false;
            }

            if (_settings.MinAmount > new decimal(0) &&
                tenderedAmount < _settings.MinAmount)
            {
               
                paymentProcessResult.CanContinueProcessing = false;
            }

            var autoSettlement = 0;
            if (_settings.AutoSendToTerminal.HasValue)
            {
                autoSettlement = _settings.AutoSendToTerminal.Value ? 1 : 0;
                
            }

            if (autoSettlement == 1)
            {
                var worker = new BackgroundWorker();

                var errorResult = true;
                var status = "";

                worker.DoWork += (s, e) =>
                {
                    if (_settings.Network.HasValue && _settings.Network.Value)
                        errorResult = _cardProcess.ProcessNetworkPayment(tenderedAmount, out status);
                    else
                        errorResult = _cardProcess.ProcessOcbc(tenderedAmount, "", out status);
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    if (!errorResult)
                    {
                        paymentProcessResult.CanContinueProcessing = false;
                    }
                    else
                    {
                        if (_cardProcess?.PaymentFrame != null)
                        {
                            paymentProcessResult.Description = JsonConvert.SerializeObject(_cardProcess.PaymentFrame);
                            if (_cardProcess.PaymentFrame.TransactionNo != null &&
                                !string.IsNullOrEmpty(_cardProcess.PaymentFrame.TransactionNo.Trim()))
                            {
                                //ticket.SetTagValue("OCBC", _cardProcess.PaymentFrame.TransactionNo);
                            }
                            else
                            {
                                
                                paymentProcessResult.CanContinueProcessing = false;
                            }
                        }
                        else
                        {
                            //_dialogService.ShowFeedback(string.Format(LoOv.G(o => Resources.No_f),
                            //    LoOv.G(o => Resources.Card)));
                            paymentProcessResult.CanContinueProcessing = false;
                        }
                    }
                };
            }
            else if(autoSettlement == 2)
            {
                paymentProcessResult = new PaymentProcessResult(dueAmount, tenderedAmount, processResult.Description);
            }
            else
            {
                paymentProcessResult.CanContinueProcessing = false;
                return paymentProcessResult;
            }

            if (!string.IsNullOrEmpty(_cardProcess.PaymentFrame.TransactionNo))
            {
                if (!string.IsNullOrEmpty(_cardProcess.PaymentFrame.CardIssuerName))
                {
                    var cardIssuer = _cardProcess.PaymentFrame.CardIssuerName;
                    cardIssuer = cardIssuer.Replace('\u0000', ' ');
                    cardIssuer = cardIssuer.Trim();


                    //var myPaymentType = _cacheService.GetPaymentTypeByName(cardIssuer);
                    //DineGoLogger.Log("PaymentType : " + cardIssuer);
                    //if (myPaymentType != null)
                    //{
                    //    DineGoLogger.Log("Change PaymentType : " + cardIssuer);
                    //    paymentProcessResult.ChangePaymentType = myPaymentType;
                    //}
                }

                //ticket.SetTagValue("OCBC", _cardProcess.PaymentFrame.TransactionNo);
            }
            _cardProcess.CompletePort();
            return paymentProcessResult;
        }


    }
}