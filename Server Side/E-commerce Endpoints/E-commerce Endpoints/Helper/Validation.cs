using System.Text.RegularExpressions;

namespace E_commerce_Endpoints.Helper
{
    private static class Validation
    {
        private static readonly Regex EmailRegex = new Regex(
    @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+" +
    @"@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?" +
    @"(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
    RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }
    }
}
