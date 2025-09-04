using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.User.Request
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
