using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Product.Request
{
    public class AddProductDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
    }
}
