using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using DinePlan.Modules.PaymentModule.PaymentProcessors.InputTronics;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Models.Ticket;
using HashGo.Core.Util;

namespace HashGo.Wpf.App.Payment.Processor.InputTronics
{
    public class OcbcProcess
    {
        private readonly ILoggingService _logService;
        private readonly SerialPort _myNetsPort;
        private bool _mbCheckData;
        private bool _mbReceiveAck;
        private bool _mbReceiveMsg;
        private bool _mbReceiveNack;
        private short _mnReceiveNackCnt;
        private string _msResponse = "";
        private readonly string _ipAddress= "";
        private readonly string _port= "";
        private string _sReceiveData;
        private string _sendData;


        public OcbcProcess(OcbcProcessorSettings settings, string baudRate, ILoggingService logService,
            string currentTerminalName)
        {
            _logService = logService;

            if (settings != null)
                if (settings.Network!=null && settings.Network.HasValue && !settings.Network.Value)
                {
                    if (!string.IsNullOrEmpty(settings.Port))
                    {
                        _myNetsPort = new SerialPort(settings.Port, 9600, Parity.None, 8, StopBits.One)
                        {
                            RtsEnable = true,
                            Handshake = Handshake.None
                        };
                        _myNetsPort.DataReceived += OcbcHandler;
                    }
                }
                else
                {
                    _ipAddress = settings.IpAddress;
                    _port = settings.Port;

                    if (!string.IsNullOrEmpty(currentTerminalName) && !string.IsNullOrEmpty(settings.TerminalAddress))
                    {
                        var myTerminalValues = settings.TerminalAddress.Split(';');
                        if (myTerminalValues.Length > 0)
                        {
                            var myTerminalIp = myTerminalValues.FirstOrDefault(a => a.StartsWith(currentTerminalName));
                            if (myTerminalIp != null)
                            {
                                var terminalSplitDetail = myTerminalIp.Split(('@'));
                                if (terminalSplitDetail.Length > 0)
                                {
                                    _ipAddress = terminalSplitDetail[1];
                                }
                            }
                        }
                    }

                }

            PaymentFrame = new PaymentFrame();
        }

        public PaymentFrame PaymentFrame { get; }


        private void OcbcHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var sp = (SerialPort)sender;
                if (!sp.IsOpen) return;

                var sCOMReceiveData = "";

                if (_mbCheckData)
                {
                    var msg = new byte[1024];
                    var msgLen = 0;
                    _mbReceiveMsg = ReceiveMsg(sp, ref msg, out msgLen);
                    _logService.Debug("Coming Inside true of _mbCheckData");
                    _mbCheckData = false;
                }
                else
                {
                    Thread.Sleep(100);
                    sCOMReceiveData = sp.ReadExisting();
                    sCOMReceiveData = DineGoUtility.HexToString(sCOMReceiveData);
                    if (sCOMReceiveData.Length > 0)
                        _logService.Debug("Check Data false - Receive Data :" + sCOMReceiveData);
                    _mbReceiveAck = sCOMReceiveData.Equals("06");
                    _logService.Debug("Coming Inside false of _mbReceiveAck" + _mbReceiveAck);
                    _mbReceiveNack = sCOMReceiveData.Equals("15");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ProcessOcbc( decimal dAmount, string refNo, out string sErrorMsg)
        {

            bool bResult = false, bLoop = true;
            short nTransactionSend = 0;
            sErrorMsg = "";
            _msResponse = "";
            try
            {
                var sECRNo = DateTime.Now.ToString("yyMMddHHmmss");

                var wStart = DateTime.Now;
                var nDurationSeconds = 0;
                DateTime wProgress;
                while (bLoop)
                {
                    //Application.DoEvents();
                    wProgress = DateTime.Now;
                    var tsDateDiff = wProgress.Subtract(wStart);
                    nDurationSeconds = (int)tsDateDiff.TotalSeconds;
                    if (nTransactionSend == 0)
                    {
                        SalesTransaction(dAmount, sECRNo, refNo, out sErrorMsg);
                        wStart = DateTime.Now;
                        nTransactionSend++;
                    }
                    else if (nTransactionSend == 1 && nDurationSeconds == 4)
                    {
                        SalesTransaction( dAmount, sECRNo, refNo, out sErrorMsg);
                        wStart = DateTime.Now;
                        nTransactionSend++;
                    }
                    else if (nTransactionSend == 2 && nDurationSeconds == 4)
                    {
                        SalesTransaction( dAmount, sECRNo, refNo, out sErrorMsg);
                        wStart = DateTime.Now;
                        nTransactionSend++;
                    }

                    //ACK
                    if (_mbReceiveAck)
                    {
                        _logService.Debug("Coming Inside _mbReceiveAck");
                        _mbReceiveAck = false;
                        _mbCheckData = true;
                        bLoop = false;
                    }
                    //NACK
                    else if (_mbReceiveNack)
                    {
                        _mnReceiveNackCnt++;
                        _mbReceiveNack = false;
                        if (_mnReceiveNackCnt == 3)
                        {
                            bLoop = false;
                        }
                        else
                        {
                            SalesTransaction( dAmount, sECRNo, refNo, out sErrorMsg);
                            wStart = DateTime.Now;
                            nTransactionSend++;
                        }
                    }
                    else if (nDurationSeconds >= 10)
                    {
                        bLoop = false;
                    }
                }

                if (!_mbCheckData)
                {
                    sErrorMsg = "Acknowledgement not received. Please check the cable!";
                    return false;
                }

                wStart = DateTime.Now;
                while (_mbCheckData)
                {
                    wProgress = DateTime.Now;
                    var tsDateDiff = wProgress.Subtract(wStart);
                    nDurationSeconds = (int)tsDateDiff.TotalSeconds;
                    if (nDurationSeconds >= 120) _mbCheckData = false;
                }

                if (!_mbReceiveMsg)
                {
                    sErrorMsg = "LRC Calculation Error!";
                }
                else
                {
                    bResult = _msResponse == "00";
                    if (!bResult) sErrorMsg = GetInputronicsErrorMsg(_msResponse);
                }
            }
            catch (Exception ex)
            {
                sErrorMsg = ex.Message;
            }
            finally
            {
                if (_myNetsPort != null && _myNetsPort.IsOpen) _myNetsPort.Close();
            }

            return bResult;
        }

        private void SalesTransaction( decimal dAmount, string sECRNo, string refNo, out string sError)
        {
            sError = "";
            try
            {
                var sLength = "";
                var sFunctionCode = "";
                var sSTX = "02";
                var sETX = "03";
                string sHeaderData = "", sMessageData = "", sSendData = "";
                var sLrcData = "";
              
                sLength = "00 43 ";
                sHeaderData += sLength;
                sHeaderData += "50 "; 
                sFunctionCode = "30 "; //Sale - Full Payment
                sHeaderData += sFunctionCode; //"33 30 "; //Function Code
                sHeaderData += "56 31 38 "; //Msg Version
                sMessageData += GetStringInHex(sECRNo, 20);
                sMessageData += GetAmountInHex(dAmount, 12);
                sMessageData += "30 30 30 30 30 30 ";
                //Header

                sSendData = sHeaderData + sMessageData + sETX;
                sLrcData = GetLrc(sSendData.Replace(" ", ""));
                sSendData = sSTX + " " + sSendData + " " + sLrcData;
                if (_myNetsPort != null)
                {
                    if (!_myNetsPort.IsOpen) _myNetsPort.Open();
                    var bytes = sSendData.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();
                    _myNetsPort.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    _sendData = sSendData;
                }

            }
            catch (Exception ex)
            {
                sError = ex.Message;
            }
        }

        private string GetStringInHex(string sReferenceNo, int nLength)
        {
            var sHex = "";
            sReferenceNo = sReferenceNo.PadLeft(nLength, '0');
            var asAmountBytes = Encoding.ASCII.GetBytes(sReferenceNo);

            foreach (var amBy in asAmountBytes)
                sHex += amBy.ToString("X2") + " ";

            return sHex;
        }

        private string GetLrc(string Data)
        {
            var checksum = 0;
            foreach (var c in GetStringFromHex(Data)) checksum ^= Convert.ToByte(c);
            var hex = checksum.ToString("X2");
            return hex;
        }

        private string GetStringFromHex(string s)
        {
            var result = "";
            var s2 = s.Replace(" ", "");
            for (var i = 0; i < s2.Length; i += 2)
                result += Convert.ToChar(int.Parse(s2.Substring(i, 2), NumberStyles.HexNumber));
            return result;
        }

        private string GetAmountInHex(decimal dAmount, int nLength)

        {
            var sHex = "";
            var sAmount = FormatNumber(dAmount).Replace(".", "");
            sAmount = sAmount.PadLeft(nLength, '0');
            var asAmountBytes = Encoding.ASCII.GetBytes(sAmount);
            for (var nCnt = 0; nCnt < asAmountBytes.Length; nCnt++) sHex += asAmountBytes[nCnt].ToString("X2") + " ";
            return sHex;
        }

        public static string FormatNumber(decimal dTemp)
        {
            var ret = "";
            try
            {
                ret = $"{dTemp:0.00}";
            }
            catch
            {
                ret = "";
            }

            return ret;
        }

        private string SplitSpaceCharacters(string s, short nNoOfCharacters = 2)
        {
            var list = Enumerable
                .Range(0, s.Length / nNoOfCharacters)
                .Select(i => s.Substring(i * nNoOfCharacters, nNoOfCharacters))
                .ToList();

            var res = string.Join(" ", list);

            return res;
        }

        private bool ReceiveMsg(SerialPort comPort, ref byte[] msg, out int msgLen)
        {
            msgLen = 0;
            var num = 0;
            var ReceiveTries = 3;
            var ReadTimeOut = 1500;
            var sCOMReceiveData = "";
            while (num < ReceiveTries)
            {
                var bytesToRead = 0;
                for (var i = 0; i < ReadTimeOut / 100; i++)
                {
                    Thread.Sleep(100);
                    if (comPort.BytesToRead > bytesToRead)
                        bytesToRead = comPort.BytesToRead;
                    else if (bytesToRead > 0) break;
                }

                if (bytesToRead != 0)
                {
                    msgLen = bytesToRead;
                    comPort.Read(msg, 0, msgLen);
                    sCOMReceiveData = DineGoUtility.HexByteToString(msg, msgLen);
                    if (sCOMReceiveData.Length > 0)
                        _logService.Debug("Check Data true - Receive Data " + sCOMReceiveData);

                    if (sCOMReceiveData.Length == 0)
                        continue;

                    _logService.Debug("ReceiveTries" + sCOMReceiveData);
                    _logService.Debug("LRC : " + GetLrc(sCOMReceiveData.Substring(2, sCOMReceiveData.Length - 4)));
                    _logService.Debug("sCom : " + sCOMReceiveData.Substring(sCOMReceiveData.Length - 2, 2));


                    var bCheckLrc = GetLrc(sCOMReceiveData.Substring(2, sCOMReceiveData.Length - 4)) ==
                                    sCOMReceiveData.Substring(sCOMReceiveData.Length - 2, 2);

                    _msResponse = GetStringFromHex(sCOMReceiveData.Substring(80, 4));

                    if (_msResponse == "00")
                    {
                        PaymentFrame.MerchantId = GetStringFromHex(sCOMReceiveData.Substring(84, 30));
                        PaymentFrame.TerminalId = GetStringFromHex(sCOMReceiveData.Substring(114, 16));
                        PaymentFrame.ApprovalCode = GetStringFromHex(sCOMReceiveData.Substring(178, 12));
                        PaymentFrame.CardNo = GetStringFromHex(sCOMReceiveData.Substring(130, 40));
                        PaymentFrame.TransactionNo = GetStringFromHex(sCOMReceiveData.Substring(210, 24));
                        PaymentFrame.CardIssuerName = GetStringFromHex(sCOMReceiveData.Substring(190, 20));
                    }

                    if (bCheckLrc)
                    {
                        _logService.Debug("bCheckLRC");
                        _myNetsPort.Write(DineGoUtility.GetStringFromHex("06"));
                        return true;
                    }

                    _myNetsPort.Write(DineGoUtility.GetStringFromHex("15"));
                    num++;
                    if (ReceiveTries == num && _msResponse == "00") return true;
                }
                else
                {
                    break;
                }
            }

            return false;
        }

        private string GetInputronicsErrorMsg(string sResponeCode)
        {
            var sErrorMsg = "Error";
            switch (sResponeCode)
            {
                case "01":
                    sErrorMsg = "Please call authorizer";
                    break;
                case "02":
                    sErrorMsg = "REFER TO BANK";
                    break;
                case "03":
                    sErrorMsg = "INVALID TERMINAL";
                    break;
                case "04":
                    sErrorMsg = "Pickup Card";
                    break;
                case "07":
                    sErrorMsg = "Pickup fraud card";
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
                    sErrorMsg = "format error";
                    break;
                case "38":
                    sErrorMsg = "allowable PIN tries exceeded.";
                    break;
                case "31":
                case "41":
                case "42":
                case "43":
                    sErrorMsg = "INVALID CARD";
                    break;
                case "51":
                    sErrorMsg = "DECLINED";
                    break;
                case "52":
                    sErrorMsg = "Current acct. not available";
                    break;
                case "53":
                    sErrorMsg = "Saving acct. not available";
                    break;
                case "54":
                    sErrorMsg = "EXPIRED";
                    break;
                case "55":
                    sErrorMsg = "INCORRECT PIN";
                    break;
                case "57":
                    sErrorMsg = "Transaction not permitted to card holder";
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
                case "94":
                    sErrorMsg = "Duplicate Transmission";
                    break;
                case "96":
                    sErrorMsg = "System Malfunction";
                    break;
                case "TA":
                    sErrorMsg = "Transaction Aborted";
                    break;
                case "TO":
                    sErrorMsg = "Timeout";
                    break;
                case "FE":
                    sErrorMsg = "Format Error";
                    break;
                case "IC":
                    sErrorMsg = "Invalid Card";
                    break;
                case "AL":
                    sErrorMsg = "Amount exceed limit";
                    break;
                default:
                    sErrorMsg = "NETS Transaction Error";
                    break;
            }

            return sErrorMsg;
        }

        public void CompletePort()
        {
            if (_myNetsPort != null && _myNetsPort.IsOpen) _myNetsPort.Close();
        }

        #region Network

        public bool ProcessNetworkPayment(decimal dAmount, out string status)
        {
            bool returnSuccess = false;
            status = "";
            try
            {
                var sEcrNumber = DateTime.Now.ToString("yyMMddHHmmss");
                string sIpAddress = _ipAddress;
                var nPortNo = Convert.ToInt32(_port);
                SalesTransaction(dAmount, sEcrNumber, "", out _);
                ConnectNetworkTerminal(sIpAddress, nPortNo);
                _logService.Debug("Message Log");
                _logService.Debug("Received Data :"+ _sReceiveData);
                _logService.Debug("Received Str  :"+ GetStringFromHex(_sReceiveData));

                if (_sReceiveData!=null && _sReceiveData.Length > 40)
                {
                    _msResponse = GetStringFromHex(_sReceiveData.Substring(80, 4));
                    _logService.Debug("Substr at 80,4 : " + _msResponse);
                    _logService.Debug("Message Log End");

                    if (_msResponse == "00")
                    {
                        PaymentFrame.MerchantId = GetStringFromHex(_sReceiveData.Substring(84, 30));
                        PaymentFrame.TerminalId = GetStringFromHex(_sReceiveData.Substring(114, 16));
                        PaymentFrame.ApprovalCode = GetStringFromHex(_sReceiveData.Substring(178, 12));
                        PaymentFrame.CardNo = GetStringFromHex(_sReceiveData.Substring(130, 40));
                        PaymentFrame.TransactionNo = GetStringFromHex(_sReceiveData.Substring(210, 24));
                        PaymentFrame.CardIssuerName = GetStringFromHex(_sReceiveData.Substring(95*2, 10*2));

                        returnSuccess = true;
                    }
                    else
                    {
                        status = GetInputronicsErrorMsg(_msResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.TraceException(ex);
            }

            return returnSuccess;
        }

        public void ConnectNetworkTerminal(System.String sIPAddress, int nPortNo)
        {
            try
            {
                // Create a TcpClient.
                TcpClient client = new TcpClient(sIPAddress, nPortNo);
                _logService.Debug("----------------Send Data ------" + _sendData);
                byte[] bytes = DineGoUtility.HexStringToByteArray(_sendData);
                NetworkStream stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);
                DateTime now = DateTime.Now;
                while (DateTime.Now.Subtract(now).Minutes < 2)
                {
                    _logService.Debug("----------------Coming Inside------");
                    byte[] data = new byte[2000];
                    System.String responseData;
                    Int32 bytesReceive = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytesReceive);
                    _sReceiveData = DineGoUtility.HexToString(responseData);
                    if (_sReceiveData.Length > 30) break;

                    _logService.Debug("----------------Coming Inside End------");

                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                _logService.TraceException(e);
            }
            catch (SocketException e)
            {
                _logService.TraceException(e);
            }
            catch (Exception e)
            {
                _logService.TraceException(e);
            }

        }


        #endregion
    }
}