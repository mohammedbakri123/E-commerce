namespace E_commerce_Endpoints.DTO.Authentication.Request
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
      
    }
}