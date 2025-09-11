using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Variant.Request
{
    public class UpdateVariantDTO
    {
        [Required]
        public int VariantId { get; set; }
        public string? ImagePaths { get; set; }
        [Required]
        public string? VariantProperties { get; set; }
        [Required]
        public bool? Status { get; set; }
    }
}
