using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.User.Request
{
    public class ChangePasswordDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }


    }
}
