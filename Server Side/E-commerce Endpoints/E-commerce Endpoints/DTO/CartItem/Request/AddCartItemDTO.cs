using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.CartItem.Request
{
    public class AddCartItemDTO
    {
        [Required]
        public int CartId { get; set; }

        [Required]
        public int VariantId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
