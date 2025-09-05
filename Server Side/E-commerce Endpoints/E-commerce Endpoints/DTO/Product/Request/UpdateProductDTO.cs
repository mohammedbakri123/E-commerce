using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Product.Request
{
    public class UpdateProductDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? BrandId { get; set; }
        [Required]
        public int? SubCategoryId { get; set; }
    }
}
