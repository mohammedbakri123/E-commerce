using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Authentication.Request
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
