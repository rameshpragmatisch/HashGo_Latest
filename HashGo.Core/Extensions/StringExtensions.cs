using System.Globalization;
using System.Text;

namespace HashGo.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToSentenseCase(this string str)
        {
            bool isNewSentense = true;

            var result = new StringBuilder(str.Length);

            foreach (char c in str)
            {
                if (isNewSentense && char.IsLetter(c))
                {
                    result.Append(char.ToUpper(c));
                    isNewSentense = false;
                }
                else
                {
                    result.Append(c);
                }

                if (c == '!' || c == '?' || c == '.')
                {
                    isNewSentense = true;
                }
            }

            return result.ToString();
        }
    }
}
