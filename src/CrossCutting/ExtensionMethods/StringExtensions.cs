using System.Net;
using System.Text.RegularExpressions;

namespace Farfetch.CrossCutting.ExtensionMethods
{
    public static class StringExtensions
    {
        #region Public methods
        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string UrlEncode(this string value)
        {
            return WebUtility.UrlEncode(value);
        }

        public static string UrlDecode(this string value)
        {
            return WebUtility.UrlDecode(value);
        }

        public static bool IsEmail(this string email)
        {
            return email.HasValue()
                && Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion
    }
}
