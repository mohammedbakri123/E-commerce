using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.CartItem.Request
{
    public class UpdateCartItemDTO
    {
        [Required]
        public int CartItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
