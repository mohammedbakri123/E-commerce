using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Supplier.Request
{
    public class AddSupplierDTO
    {
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = null!;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}
