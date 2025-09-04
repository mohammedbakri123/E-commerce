using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace E_commerce_Endpoints.Helper
{
    public static class Validation
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
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Minimum 8 chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special character (anything non-alphanumeric)
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$");

            return regex.IsMatch(password);
        }

        public static bool TryValidate<T>(T dto, out List<ValidationResult> errors)
        {
            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            errors = new List<ValidationResult>();
            return Validator.TryValidateObject(dto, context, errors, validateAllProperties: true);
        }
    }
}
