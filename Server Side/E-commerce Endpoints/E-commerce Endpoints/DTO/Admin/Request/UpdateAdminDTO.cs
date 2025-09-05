using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Admin.Request
{
    public class UpdateAdminDTO
    {
        public int AdminId { get; set; }

        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string? AdminLastName { get; set; }

        public int? PermissionsValue { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string? Address { get; set; }
    }
}
