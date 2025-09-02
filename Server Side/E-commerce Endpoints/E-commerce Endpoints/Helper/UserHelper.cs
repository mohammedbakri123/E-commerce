namespace E_commerce_Endpoints.Helper
{
    public static class UserHelper
    {
        public static string GetUserName(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return string.Empty;
            return email.Split('@')[0];
        }
    }
}
