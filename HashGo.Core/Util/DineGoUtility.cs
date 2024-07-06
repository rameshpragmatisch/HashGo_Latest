#region using

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace HashGo.Core.Util
{
    public static class DineGoUtility
    {
        //37
        public static readonly string[] NumberArray_en =
        {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN",
            "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
            "EIGHTEEN", "NINETEEN", "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY",
            "HUNDRED", "THOUSAND", "MILLION", "BILLION", "ONLY", "AND", "CENTS", " ", "DOLLARS"
        };

        public static bool IsTextNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        public static readonly string[] NumberArray_th =
        {
            "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด",
            "แปด", "เก้า", "สิบ", "สิบเอ็ด", "สิบสอง", "สิบสาม", "สิบสี่", "สิบห้า", "สิบหก", "สิบเจ็ด", "สิบแปด",
            "สิบเก้า", "ยี่สิบ", "สามสิบ", "สี่สิบ", "ห้าสิบ", "หกสิบ", "เจ็ดสิบ", "แปดสิบ", "เก้าสิบ", "ร้อย", "พัน",
            "ล้าน", "พันล้าน", "เท่านั้น", "และ", "สตางค์", "", "บาท"
        };

        public static string FormatBracketRegex(string pattern)
        {
            var str = pattern.Substring(0, pattern.Length - 1);
            var chr = pattern[0];
            var chr1 = pattern[pattern.Length - 1];
            return string.Format("{2}(([^\\{0}\\{1}]+|(?<Level>\\{0})|(?<-Level>\\{1}))+(?(Level)(?!)))\\{1}", chr,
                chr1,
                str.Aggregate("", (current, c) => string.Concat(current, "\\", c)));
        }

        public static string AddTypedValue(string actualValue, string typedValue, string format)
        {
            decimal amnt;
            var stringMode = false;
            decimal.TryParse(actualValue, out amnt);
            if (actualValue.EndsWith("-") || amnt == 0 || actualValue.StartsWith("0"))
            {
                stringMode = true;
            }
            else
            {
                decimal.TryParse(typedValue, out amnt);
                if (amnt == 0) stringMode = true;
            }

            var dc = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (typedValue == "." || typedValue == ",")
            {
                actualValue += dc;
                return actualValue;
            }

            if (stringMode)
                return actualValue + typedValue;

            var fmt = "0";
            var rfmt = format;

            if (actualValue.Contains(dc))
            {
                var dCount = actualValue.Length - actualValue.IndexOf(dc);

                fmt = "0.".PadRight(dCount + 2, '0');
                rfmt = format.PadRight(dCount + rfmt.Length, '0');
            }

            var amount = string.IsNullOrEmpty(actualValue)
                ? "0"
                : Convert.ToDouble(actualValue).ToString(fmt);
            if (amount.Contains(dc))
                amount = amount.Substring(0, amount.Length - 1);

            var dbl = Convert.ToDouble(amount + typedValue);
            return dbl.ToString(rfmt);
        }

        public static decimal ConvertToDecimal(string priceStr)
        {
            decimal num;
            var numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            try
            {
                priceStr = priceStr.Replace(".", numberDecimalSeparator);
                priceStr = priceStr.Replace(",", numberDecimalSeparator);
                num = ToDecimal(priceStr, new decimal(0));
            }
            catch
            {
                num = new decimal(0);
            }

            return num;
        }

        public static decimal ToDecimal(string value,
            [DecimalConstant(0, 0, 0, 0, 0)] decimal defaultValue = default)
        {
            decimal num;
            if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out num)) return defaultValue;
            return num;
        }

        public static decimal GetDecimal(string numstring)
        {
            decimal ret = 0;
            try
            {
                ret = Convert.ToDecimal(numstring.Trim());
            }
            catch
            {
                ret = 0;
            }

            return ret;
        }

        public static decimal ToDecimal(object value,
            [DecimalConstant(0, 0, 0, 0, 0)] decimal defaultValue = default)
        {
            decimal num;
            if (!decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out num))
                return defaultValue;
            return num;
        }

        public static string AddDoubleKeyPadValue(string actual, string current)
        {
            var dc = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (actual.Equals("") && current.Equals("100")) return "100.00";

            if (current.Equals(".")) return actual;

            if (actual.Equals("")) return "0.0" + current;
            if (actual.StartsWith("0"))
            {
                var firstOut = actual.Substring(1, actual.Length - 1);
                var afterDecimal = firstOut.Substring(firstOut.IndexOf(dc) + 1, 2);

                if (afterDecimal.StartsWith("0"))
                {
                    var chuckOut = afterDecimal.Substring(1, afterDecimal.Length - 1);

                    if (current.Length > 1) return chuckOut + "." + current;
                    return "0." + chuckOut + current;
                }

                if (current.Length > 1) return afterDecimal + "." + current;
                return afterDecimal.Substring(0, afterDecimal.Length - 1) + "." +
                       afterDecimal.Substring(afterDecimal.Length - 1) + current;
            }
            else
            {
                var decimalPosition = actual.IndexOf(dc);
                var beforeDecimal = actual.Substring(0, decimalPosition);
                var afterDecimal = actual.Substring(decimalPosition + 1);
                var finalString = beforeDecimal + afterDecimal;

                if (current.Length > 1) return finalString + "." + current;
                return finalString.Substring(0, finalString.Length - 1) + "." +
                       finalString.Substring(finalString.Length - 1) + current;
            }
        }


        public static bool IsValidFile(string fileName)
        {
            fileName = fileName.Trim();
            if (fileName == "." || !fileName.Contains(".")) return false;
            var result = false;
            try
            {
                new FileInfo(fileName);
                result = true;
            }
            catch (ArgumentException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (NotSupportedException)
            {
            }

            return result;
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null) return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    return false;
            }

            return false;
        }

        public static int GenerateCheckDigit(string idWithoutCheckdigit)
        {
            const string validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVYWXZ_";
            idWithoutCheckdigit = idWithoutCheckdigit.Trim().ToUpper();

            var sum = 0;

            for (var i = 0; i < idWithoutCheckdigit.Length; i++)
            {
                var ch = idWithoutCheckdigit[idWithoutCheckdigit.Length - i - 1];
                if (validChars.IndexOf(ch) == -1)
                    throw new Exception(ch + " is an invalid character");
                var digit = ch - 48;
                int weight;
                if (i % 2 == 0)
                    weight = 2 * digit - digit / 5 * 9;
                else
                    weight = digit;
                sum += weight;
            }

            sum = Math.Abs(sum) + 10;
            return (10 - sum % 10) % 10;
        }

        public static bool ValidateCheckDigit(string id)
        {
            if (id.Length < 2) return false;
            var cd = Convert.ToInt32(id.Last().ToString());
            return cd == GenerateCheckDigit(id.Remove(id.Length - 1));
        }

        public static string RandomString(int length,
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
                throw new ArgumentException($"allowedChars may contain no more than {byteSize} characters.");

            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - byteSize % allowedCharSet.Length;
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }

        public static string GetDateBasedUniqueString()
        {
            var date = DateTime.Now;
            return $"{date.Year}{date.Month:00}{date.Day:00}{date.Hour:00}{date.Minute:00}{date.Millisecond:000}";
        }

        public static bool IsDecimal(string value)
        {
            decimal num;
            return decimal.TryParse(value, out num);
        }

        public static bool HasDecimal(string value)
        {
            decimal num;
            var isde = decimal.TryParse(value, out num);
            if (isde)
            {
                const int expMask = 0x00FF0000;
                var hasDecimal = (decimal.GetBits(Convert.ToDecimal(value))[3] & expMask) != 0x0;
                return hasDecimal;
            }

            return false;
        }

        public static bool IsInteger(string value)
        {
            return int.TryParse(value, out int num);
        }

        public static DateTime? StringToLocalDate(string value)
        {
            DateTime dateTime;
            if (DateTime.TryParse(value, out dateTime)) return dateTime;
            return null;
        }

        public static string StringToLocalDateString(string value)
        {
            var localDate = StringToLocalDate(value);
            if (!localDate.HasValue) return value;
            return localDate.GetValueOrDefault().ToShortDateString();
        }

        public static DateTime? StrToDate(string value)
        {
            DateTime dateTime;
            DateTime? nullable;
            if (string.IsNullOrEmpty(value.Trim())) return null;
            try
            {
                char[] chrArray = { ' ' };
                var list = (
                    from x in value.Split(chrArray, StringSplitOptions.RemoveEmptyEntries)
                    select Convert.ToInt32(x)).ToList();
                if (list.Count == 1) list.Add(DateTime.Now.Month);
                if (list.Count == 2) list.Add(DateTime.Now.Year);
                if (list[2] < 1) list[2] = DateTime.Now.Year;
                if (list[2] < 1000)
                {
                    var item = list;
                    item[2] = item[2] + 2000;
                }

                if (list[1] < 1) list[1] = 1;
                if (list[1] > 12) list[1] = 12;
                var num = DateTime.DaysInMonth(list[2], list[1]);
                if (list[0] < 1) list[0] = 1;
                if (list[0] > num) list[0] = num;
                nullable = new DateTime(list[2], list[1], list[0]);
            }
            catch
            {
                DateTime.TryParse(value, out dateTime);
                nullable = dateTime;
            }

            return nullable;
        }

        public static double ToDouble(string value, double defaultValue = 0)
        {
            double num;
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out num)) return defaultValue;
            return num;
        }

        public static int ToInteger(string value, int defaultValue = 0)
        {
            int num;
            if (!int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out num)) return defaultValue;
            return num;
        }
        public static int ConvertToNumber(string value)
        {
            if (value == string.Empty) return 0;

            try
            {
                var dec = decimal.Parse(value,
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.Number |
                    NumberStyles.AllowThousands);

                return (int)Math.Round(dec);
            }
            catch (Exception)
            {
                Console.WriteLine("Please input a number.");
                return 0;
            }
        }
        #region Base64

        public static string EncodeTo64(string toEncode)

        {
            var toEncodeAsBytes
                = Encoding.ASCII.GetBytes(toEncode);

            var returnValue
                = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        #endregion

        public static bool MatchtheString(string paymentName, string left)
        {
            return paymentName.ToUpper().Equals(left.ToUpper());
        }

        public static void BrowseUrl(string url)
        {
            Process.Start(url);
        }

        #region Utilities

        public static string HexToString(string input)
        {
            string hexOutput = "", sHexaData = "";
            var values = input.ToCharArray();
            foreach (var letter in values)
            {
                // Get the integral value of the character.
                var value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                sHexaData = $"{value:X}";
                if (sHexaData.Length == 1) sHexaData = "0" + sHexaData;
                hexOutput = hexOutput + sHexaData;
            }

            return hexOutput;
        }

        public static string HexByteToString(byte[] byte_buffer, int nLength)
        {
            string hexOutput = "", sHexaData = "";

            for (var nCnt = 0; nCnt < nLength; nCnt++)
            {
                sHexaData = string.Format("{0:X}", byte_buffer[nCnt]);
                if (sHexaData.Length == 1) sHexaData = "0" + sHexaData;
                hexOutput = hexOutput + sHexaData;
            }

            return hexOutput;
        }

        public static string GetAmountInHex(decimal dAmount, int nLength)
        {
            var sHex = "";
            var sAmount = FncFormatNumber(dAmount).Replace(".", "");
            sAmount = sAmount.PadLeft(12, '0');
            var asAmountBytes = Encoding.ASCII.GetBytes(sAmount);

            for (var nCnt = 0; nCnt < asAmountBytes.Length; nCnt++) sHex += asAmountBytes[nCnt].ToString("X2") + " ";
            return sHex;
        }

        public static string FncFormatNumber(decimal dTemp)
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

        public static string ConvertStringToHex(string input)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                if (b == 0) continue;
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public static string GetStringInHex(string sReferenceNo, int nLength=0)
        {
            var sHex = "";
            if(nLength>0)
                sReferenceNo = sReferenceNo.PadLeft(nLength, '0');

            var asAmountBytes = Encoding.ASCII.GetBytes(sReferenceNo);

            foreach (var t in asAmountBytes)
                sHex += t.ToString("X2") + " ";

            return sHex;
        }

        public static string GetLrc(string Data)
        {
            var checksum = 0;
            foreach (var c in GetStringFromHex(Data)) checksum ^= Convert.ToByte(c);
            var hex = checksum.ToString("X2");
            return hex;
        }

        public static string GetStringFromHex(string s)
        {
            var result = "";
            var s2 = s.Replace(" ", "");
            for (var i = 0; i < s2.Length; i += 2)
                result += Convert.ToChar(int.Parse(s2.Substring(i, 2), NumberStyles.HexNumber));
            return result;
        }
        public static byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");

            return Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }
        #endregion

        #region ToWords

        public static string ConvertToWords(string numb, string paisaCode, string countryCode)
        {
            var converstionArray = NumberArray_en;
            paisaCode = converstionArray[34];
            string val = "", wholeNo = numb;
            string andStr = "", pointStr = "";
            var endStr = converstionArray[32];
            try
            {
                var decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    var points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = converstionArray[33];
                        endStr = paisaCode + converstionArray[35] + endStr;
                        pointStr = ConvertDecimals(points, converstionArray);
                    }
                }

                val =
                    $"{ConvertWholeNumber(wholeNo, converstionArray).Trim()}{converstionArray[35]}{converstionArray[36]} {andStr}{converstionArray[35]}{pointStr}{converstionArray[35]}{endStr}";
            }
            catch
            {
            }

            return val;
        }

        private static string ConvertWholeNumber(string number, string[] converstionArray)
        {
            var word = "";
            try
            {
                var isDone = false;
                var dblAmt = Convert.ToDouble(number);
                if (dblAmt > 0)
                {
                    var numDigits = number.Length;
                    var pos = 0;
                    var place = "";
                    switch (numDigits)
                    {
                        case 1: //ones' range

                            word = Ones(number, converstionArray);
                            isDone = true;
                            break;
                        case 2: //tens' range
                            word = Tens(number, converstionArray);
                            isDone = true;
                            break;
                        case 3: //hundreds' range
                            pos = numDigits % 3 + 1;
                            place = converstionArray[35] + converstionArray[28] + " ";
                            break;
                        case 4: //thousands' range
                        case 5:
                        case 6:
                            pos = numDigits % 4 + 1;
                            place = converstionArray[35] + converstionArray[29] + " ";
                            break;
                        case 7: //millions' range
                        case 8:
                        case 9:
                            pos = numDigits % 7 + 1;
                            place = converstionArray[35] + converstionArray[30] + " ";
                            break;
                        case 10: //Billions's range
                        case 11:
                        case 12:

                            pos = numDigits % 10 + 1;
                            place = converstionArray[35] + converstionArray[31] + " ";
                            break;
                        default:
                            isDone = true;
                            break;
                    }

                    if (!isDone)
                    {
                        if (number.Substring(0, pos) != "0" && number.Substring(pos) != "0")
                            try
                            {
                                word = ConvertWholeNumber(number.Substring(0, pos), converstionArray) + place +
                                       ConvertWholeNumber(number.Substring(pos), converstionArray);
                            }
                            catch
                            {
                                // ignored
                            }
                        else
                            word = ConvertWholeNumber(number.Substring(0, pos), converstionArray) +
                                   ConvertWholeNumber(number.Substring(pos), converstionArray);
                    }

                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch
            {
            }

            return word.Trim();
        }

        private static string Tens(string number, string[] converstionArray)
        {
            var _Number = Convert.ToInt32(number);
            string name = null;
            switch (_Number)
            {
                case 10:
                    name = converstionArray[10];
                    break;
                case 11:
                    name = converstionArray[11];
                    break;
                case 12:
                    name = converstionArray[12];
                    break;
                case 13:
                    name = converstionArray[13];
                    break;
                case 14:
                    name = converstionArray[14];
                    break;
                case 15:
                    name = converstionArray[15];
                    break;
                case 16:
                    name = converstionArray[16];
                    break;
                case 17:
                    name = converstionArray[17];
                    break;
                case 18:
                    name = converstionArray[18];
                    break;
                case 19:
                    name = converstionArray[19];
                    break;
                case 20:
                    name = converstionArray[20];
                    break;
                case 30:
                    name = converstionArray[21];
                    break;
                case 40:
                    name = converstionArray[22];
                    break;
                case 50:
                    name = converstionArray[23];
                    break;
                case 60:
                    name = converstionArray[24];
                    break;
                case 70:
                    name = converstionArray[25];
                    break;
                case 80:
                    name = converstionArray[26];
                    break;
                case 90:
                    name = converstionArray[27];
                    break;
                default:
                    if (_Number > 0)
                        name = Tens(number.Substring(0, 1) + "0", converstionArray) + " " +
                               Ones(number.Substring(1), converstionArray);
                    break;
            }

            return name;
        }

        private static string Ones(string Number, string[] converstionArray)
        {
            var number = Convert.ToInt32(Number);
            var name = "";
            switch (number)
            {
                case 1:
                    name = converstionArray[1];
                    break;
                case 2:
                    name = converstionArray[2];
                    break;
                case 3:
                    name = converstionArray[3];
                    break;
                case 4:
                    name = converstionArray[4];
                    break;
                case 5:
                    name = converstionArray[5];
                    break;
                case 6:
                    name = converstionArray[6];
                    break;
                case 7:
                    name = converstionArray[7];
                    break;
                case 8:
                    name = converstionArray[8];
                    break;
                case 9:
                    name = converstionArray[9];
                    break;
            }

            return name;
        }

        private static string ConvertDecimals(string number, string[] converstionArray)
        {
            return ConvertWholeNumber(number, converstionArray);
        }

        #endregion

        public static string HideCardInfo(string cardNo)
        {
            var chars = cardNo.ToCharArray();
            var sb = new StringBuilder();
            
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 4 || i == 8 || i == 12)
                    sb.Append(" ");
                
                if (i < chars.Length - 8)
                    sb.Append("x");
                else
                    sb.Append(chars[i]);
            }

            return sb.ToString();
        }

        public static string HidePhoneInfo(string phone)
        {
            var chars = phone.ToCharArray();
            var sb = new StringBuilder();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 3 || i == 7)
                    sb.Append(" ");

                if (i > 2 && i < 7)
                    sb.Append("x");
                else
                    sb.Append(chars[i]);
            }

            return sb.ToString();
        }

        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}