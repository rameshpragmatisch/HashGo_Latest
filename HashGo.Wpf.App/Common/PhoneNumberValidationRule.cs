using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Common
{
    public class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if(string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Phone number cannot be empty");
            }

            if (input.Length != 8 || !(input.StartsWith("8") || input.StartsWith("9") || input.StartsWith("6")))
            {
                return new ValidationResult(false, "Phone number must be 8 digits and start with 8 or 9 or 6.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
