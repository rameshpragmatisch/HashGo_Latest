using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.Helper;
using HashGo.Domain.Services;
using HashGo.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ProcessingPaymentPage.xaml
    /// </summary>
    public partial class ProcessingPaymentPage : Page
    {
        DispatcherTimer timer;
        INavigationService navigationService;
        Dictionary<string, string> responseData = new Dictionary<string, string>()
        {
            {"EDCMode", ""},
            {"ApprovalCode", ""},
            {"TransactionNo", ""},
            {"TerminalId", ""},
            {"MerchantId", ""},
            {"CardIssuerName", ""},
            {"CardHolderName", ""},
            {"CardNo", ""},
            {"CardExpiry", ""},
            {"BatchNo", ""},
            {"ReferenceNo", ""},
            {"RRNNo", ""},
            {"EntryType", ""},
            {"InvoiceNo", ""},
            {"AppId", ""},
            {"TVRNo", ""},
            {"ResponseText", ""}
        };

        string sBank = "DBS";
        private bool mbTransactionSuccess = false;
        IRetailConnectService retailConnectService;
        public ProcessingPaymentPage(INavigationService navigationService, IRetailConnectService retailConnectService)
        {
            InitializeComponent();

            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;

            this.Loaded += (sender, e) =>
            {

                if (ApplicationStateContext.PaymentMethodObject != null && ApplicationStateContext.SalesOrderRequestObject != null)
                {
                    ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.SalesOrderRequestObject.salesOrder.netTotal);
                    DoTransaction();
                }

                ApplicationStateContext.TransactionNo = transactionNo;
                navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString());

                //timer = new DispatcherTimer()
                //{
                //    Interval = TimeSpan.FromSeconds(5),
                //};

                //timer.Tick += (sender, e) =>
                //{
                //    navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString());
                //};

                //timer.Start();
            };

            this.Unloaded += (sender, e) =>
            {
                timer.Stop();
                timer = null;
            };
        }

        #region Region Payment Transaction

        public void ProcessNetsNetwork(string sPayMode, decimal dAmount)
        {
            string sIPAddress = "127.0.0.1";
            try
            {
                string sECRNumber = DateTime.Now.ToString("yyMMddHHmmss");
                //string sIPAddress = NetsSettings.IPAddress;
                int nPortNo = 3000;
                //nPortNo = Convert.ToInt32(NetsSettings.PortNo);
                FillSendData(sPayMode, dAmount, sECRNumber);
                bool bTimeOut = false;
                ConnectNetworkTerminal(sIPAddress, nPortNo, ref bTimeOut);
                //send data again if the timeout occurs.
                //if (bTimeOut || sReceiveData == null)
                //{
                //    Application.DoEvents();
                //    Thread.Sleep(10000);
                //    ConnectNetworkTerminal(sIPAddress, nPortNo, ref bTimeOut);
                //}
                if (!bTimeOut)
                {
                    //do nothing
                }
                else
                {
                    //Application.DoEvents();
                    //resend it after 40 seconds due to network disturbance
                    Thread.Sleep(60 * 1000);
                    ConnectNetworkTerminal(sIPAddress, nPortNo, ref bTimeOut);
                }
                //ocalSettings.WriteLog("Receive Data - " + sReceiveData);
                if (sReceiveData.Length > 40)
                {
                    msResponse = Utility.GetStringFromHex(sReceiveData.Substring(32, 4));
                    if (msResponse == "00")
                    {
                        mbTransactionSuccess = true;
                        msTransactionStatus = "SUCCESS";
                        FillResponseData(sReceiveData);
                    }
                    else
                    {
                        msTransactionStatus = GetNETSErrorMsg(msResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                //LocalSettings.WriteLog(ex.Message.ToString());
            }

        }

        private string GetNETSErrorMsg(string sResponeCode)
        {
            string sErrorMsg = "";
            switch (sResponeCode)
            {
                case "01":
                    sErrorMsg = "REFER TO NETS";
                    break;
                case "02":
                    sErrorMsg = "REFER TO BANK";
                    break;
                case "03":
                    sErrorMsg = "INVALID TERMINAL";
                    break;
                case "12":
                    sErrorMsg = "INVALID TRANS";
                    break;
                case "13":
                    sErrorMsg = "INVALID AMOUNT";
                    break;
                case "14":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "19":
                    sErrorMsg = "RE-ENTER TRANSACTION";
                    break;
                case "25":
                    sErrorMsg = "NO RECORD ON FILE";
                    break;
                case "30":
                    sErrorMsg = "REFER TO NETS";
                    break;
                case "31":
                case "42":
                case "43":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "51":
                    sErrorMsg = "DECLINED";
                    break;
                case "54":
                    sErrorMsg = "EXPIRED";
                    break;
                case "55":
                    sErrorMsg = "INCORRECT PIN";
                    break;
                case "58":
                    sErrorMsg = "INVALID TRANSACTION";
                    break;
                case "61":
                    sErrorMsg = "DAILY LIMIT EXCEEDED";
                    break;
                case "62":
                    sErrorMsg = "INVALID TRANSACTION";
                    break;
                case "63":
                    sErrorMsg = "VOID IMPOSSIBLE";
                    break;
                case "64":
                    sErrorMsg = "TXN ALREADY VOID";
                    break;
                case "65":
                    sErrorMsg = "VOID IMPOSSIBLE";
                    break;
                case "75":
                    sErrorMsg = "PIN ERROR, REFER TO BANK";
                    break;
                case "76":
                case "86":
                    sErrorMsg = "DECLINED";
                    break;
                case "78":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "79":
                    sErrorMsg = "SUPERVISOR PIN ERROR";
                    break;
                case "80":
                case "81":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "82":
                    sErrorMsg = "PIN ERROR, REFER TO NETS";
                    break;
                case "85":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "87":
                    sErrorMsg = "DAILY LIMIT EXCEEDED";
                    break;
                case "88":
                    sErrorMsg = "NO MERCH RET";
                    break;
                case "89":
                    sErrorMsg = "INVALID TERMINAL";
                    break;
                case "91":
                    sErrorMsg = "NO REPLY FROM BANK";
                    break;
                case "98":
                    sErrorMsg = "MAC ERROR";
                    break;
                case "IM":
                    sErrorMsg = "UNAUTHORISED RESPONSE";
                    break;
                case "IR":
                    sErrorMsg = "INVALID HOST MESSAGE";
                    break;
                case "IT":
                    sErrorMsg = "INVALID TERMINAL";
                    break;
                case "IA":
                    sErrorMsg = "INVALID HOST AMOUNT";
                    break;
                case "IC":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "IL":
                    sErrorMsg = "INVALID DATA LENGTH";
                    break;
                case "HS":
                    sErrorMsg = "CONNECT PROBLEM TO HOST";
                    break;
                case "TO":
                    sErrorMsg = "TIMEOUT-PLEASE TRY AGAIN";
                    break;
                case "US":
                    sErrorMsg = "CANCELLED BY USER";
                    break;
                case "BF":
                    sErrorMsg = "TRANSACTION BATCH FULL";
                    break;
                case "SC":
                    sErrorMsg = "CASHCARD TRANSACTION UNSUCCESSFUL";
                    break;
                case "N0":
                    sErrorMsg = "DIFFERENT ISSUERS FOR A NETS SALE OR REVALUTION";
                    break;
                case "N1":
                    sErrorMsg = "CASHCARD BLACKLIST";
                    break;
                case "N2":
                    sErrorMsg = "BATCH ALREADY UPLOADED";
                    break;
                case "N3":
                    sErrorMsg = "RESEND BATCH";
                    break;
                case "N4":
                    sErrorMsg = "CASHCARD NOT FOUND";
                    break;
                case "N5":
                    sErrorMsg = "EXPIRED";
                    break;
                case "N6":
                    sErrorMsg = "REFUNDED CASHCARD";
                    break;
                case "N7":
                    sErrorMsg = "CERTIFICATE ERROR";
                    break;
                case "N8":
                    sErrorMsg = "INSUFFICIENT FUNDS/BALANCE";
                    break;
                case "IS":
                    sErrorMsg = "INVALID STAN*";
                    break;
                case "RN":
                    sErrorMsg = "RECORD NOT FOUND";
                    break;
                case "RE":
                    sErrorMsg = "READER NOT READY";
                    break;
                case "T1":
                    sErrorMsg = "INVALID TOP-UP CARD";
                    break;
                case "T2":
                    sErrorMsg = "TERMINAL TOP-UP LIMIT EXCEEDED";
                    break;
                case "T3":
                    sErrorMsg = "RETAILER LIMIT EXCEEDED";
                    break;
                case "LR":
                    sErrorMsg = "MANUAL LOGON REQUIRED**";
                    break;
                case "DK":
                    sErrorMsg = "BLOCKLIST DOWNLOAD REQUIRED";
                    break;
                case "DS":
                    sErrorMsg = "CASHCARD SETTLEMENT REQUIRED";
                    break;
                case "BP":
                    sErrorMsg = "CARD BLOCKED";
                    break;
                case "BA":
                    sErrorMsg = "TRANSACTION BLOCKED, to prevent Autoload";
                    break;
                case "GB":
                    sErrorMsg = "Golden Bullet Card";
                    break;
                default:
                    sErrorMsg = "NETS Transaction Error";
                    break;
            }
            return (sErrorMsg);
        }

        private void FillResponseData(string sReceiveData)
        {
            try
            {
                string sTemp = "";
                if (sReceiveData.IndexOf("443115") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("443115") + 6, 30));
                    responseData["MerchantId"] = sTemp;
                }
                if (sReceiveData.IndexOf("313608") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("313608") + 6, 16));
                    responseData["TerminalId"] = sTemp;
                }
                if (sReceiveData.IndexOf("303106") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("303106") + 6, 12));
                    responseData["ApprovalCode"] = sTemp;
                }
                if (sReceiveData.IndexOf("333016") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("333016") + 6, 32));
                    responseData["CardNo"] = sTemp;
                }
                if (sReceiveData.IndexOf("443210") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("443210") + 6, 20));
                    responseData["CardIssuerName"] = sTemp;
                }
                if (sReceiveData.IndexOf("363506") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("363506") + 6, 12));
                    responseData["TransactionNo"] = sTemp;
                }
                if (sReceiveData.IndexOf("394806") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("394806") + 6, 12));
                    responseData["InvoiceNo"] = sTemp;
                }
                if (sReceiveData.IndexOf("443312") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("443312") + 6, 24));
                    responseData["RRNNo"] = sTemp;
                }
                if (sReceiveData.IndexOf("434E02") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("434E02") + 6, 4));
                    if (sTemp == "10")
                    {
                        sTemp = "Chip Card";
                    }
                    else if (sTemp == "20")
                    {
                        sTemp = "Contactless";
                    }
                    else sTemp = "Swipe";
                    responseData["EntryType"] = sTemp;
                }

                if (sReceiveData.IndexOf("303240") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("30320040") + 8, 80));
                    responseData["ResponseText"] = sTemp;
                }
                if (sReceiveData.IndexOf("4C3720") != -1)
                {
                    sTemp = Utility.GetStringFromHex(sReceiveData.Substring(sReceiveData.IndexOf("4C3720") + 6, 40));
                    responseData["CardHolderName"] = sTemp;
                }

            }
            catch (Exception ex)
            {
                //LocalSettings.WriteLog(ex.Message.ToString());
            }
        }

        private string sSendData, sReceiveData = "", msTransactionStatus = "", msResponse = "";

        private void FillSendData(string sType, decimal dAmount, string sECRNumber)
        {
            string sLRCData = "";
            sSendData = "";
            switch (sType)
            {
                case "NETS":
                    sSendData += "00 84 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "33 30 " + "30 31 30 1C ";                             //function code (30)
                    sSendData += "54 32 00 02 " + "30 31" + " 1C ";                     //Type - 01.
                    sSendData += "34 33 00 01 30 1C ";                                  //Loyalty redemption
                    sSendData += "34 30 00 12 " + Utility.GetAmountInHex(dAmount, 12) + "1C ";  //Amount
                    sSendData += "34 32 00 12 30 30 30 30 30 30 30 30 30 30 30 30 1C "; //Cash Back
                    sSendData += "48 44 00 13 " + Utility.GetStringInHex(sECRNumber, 13) + "1C "; //ECR in Detail
                    break;
                case "NETSQR":
                    sSendData += "00 66 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "33 30 " + "30 31 30 1C ";                             //function code (30)
                    sSendData += "54 32 00 02 " + "30 34" + " 1C ";                     //Type - 04.
                    sSendData += "34 33 00 01 30 1C ";                                  //Loyalty redemption
                    sSendData += "34 30 00 12 " + Utility.GetAmountInHex(dAmount, 12) + "1C ";  //Amount
                    sSendData += "34 32 00 12 30 30 30 30 30 30 30 30 30 30 30 30 1C "; //Cash Back
                    break;
                case "NETSFP":
                    sSendData += "00 51 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "32 34 " + "30 31 30 1C ";                             //function code (24)
                    sSendData += "34 30 00 12 " + Utility.GetAmountInHex(dAmount, 12) + "1C ";  //Amount
                    sSendData += "48 44 00 10 " + Utility.GetStringInHex(sECRNumber.Substring(0, 10), 10) + "1C "; //ECR in Detail
                    break;
                case "NETSCC":
                    sSendData += "00 36 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "35 31 " + "30 31 30 1C ";                             //function code (51)
                    sSendData += "34 30 00 12 " + Utility.GetAmountInHex(dAmount, 12) + "1C "; //Amount
                    break;
                case "CREDITCARD":
                    sSendData += "00 47 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "49 30 " + "30 31 30 1C ";                             //function code (I0)
                    sSendData += "34 30 00 12 " + Utility.GetAmountInHex(dAmount, 12) + "1C ";  //Amount
                    sSendData += "39 47 00 06 " + Utility.GetStringInHex(sBank, 6) + "1C ";     //Bank
                    break;
                case "NETS_LOGON":
                    sSendData += "00 19 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "38 30 " + "30 31 30 1C ";                             //function code (80)
                    break;
                case "NETS_SETTLEMENT":
                    sSendData += "00 19 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "38 31 " + "30 31 30 1C ";                             //function code (81)
                    break;
                case "NETS_TERMINALMAINTENANCE":
                    sSendData += "00 19 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "38 34 " + "30 31 30 1C ";                             //function code (84)
                    break;
                case "NETS_CREDITCARDSETTLEMENT":
                    sSendData += "00 30 ";                                              //length
                    sSendData += Utility.GetStringInHex(sECRNumber, 12);                        //ECR
                    sSendData += "49 35 " + "30 31 30 1C ";                             //function code (81)
                    sSendData += "39 47 00 06 " + Utility.GetStringInHex(sBank, 6) + "1C ";     //Bank
                    break;
            }
            sSendData = sSendData.Replace(" ", "");
            if (sSendData.Length <= 4) return;
            sLRCData = Utility.GetLrc(sSendData.Substring(4, sSendData.Length - 4));
            sSendData += sLRCData;
        }

        public void ConnectNetworkTerminal(string sIPAddress, int nPortNo, ref bool bTimeOut)
        {
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            try
            {
                bTimeOut = false;
                // Create a TcpClient.
                client.SendTimeout = 120 * 1000;
                client.ReceiveTimeout = 60 * 1000;

                client.Connect(sIPAddress, nPortNo);
                // Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                byte[] bytes = Utility.HexStringToByteArray(sSendData);

                stream = client.GetStream();

                stream.ReadTimeout = 40 * 1000;

                // Send the message to the connected TcpServer. 
                stream.Write(bytes, 0, bytes.Length);
                //LocalSettings.WriteLog("Send Data - " + sSendData);

                //mbNetworkConnection = true;
                //PollByTimers();

                //stream.ReadTimeout = 2000;
                //client.ReceiveTimeout = 2000;

                // Buffer to store the response bytes.
                byte[] data = new byte[2000];
                // String to store the response ASCII representation.
                string responseData = string.Empty;
                // Read the first batch of the TcpServer response bytes.
                Int32 bytesReceive = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytesReceive);
                sReceiveData = Utility.ConvertStringToHex(responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (IOException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                bTimeOut = true;
                //LocalSettings.WriteLog("IO exception - timeout " + e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                //LocalSettings.WriteLog("Socket error " + e);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (client != null) client.Close();
                //tmrSync.Stop();
            }
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        async void DoTransaction()
        {
            CreateTransaction(ApplicationStateContext.SalesOrderRequestObject);
        }

        private string transactionNo;

        private async Task CreateTransaction(SalesOrderRequest salesOrderRequest)
        {
            TransactionDetails transactionDetails = await retailConnectService.CreateSalesOrderWithPayment(salesOrderRequest);

            if (transactionDetails != null)
            {
                transactionNo = transactionDetails.transactionNo;
            }
        }

        #endregion
    }
}
