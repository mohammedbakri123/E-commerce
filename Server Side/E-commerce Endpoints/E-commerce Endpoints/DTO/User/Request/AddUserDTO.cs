using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.User.Request
{
    public class AddUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
