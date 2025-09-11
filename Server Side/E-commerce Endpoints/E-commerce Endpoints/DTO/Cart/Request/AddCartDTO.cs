using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Cart.Request
{
    public class AddCartIDTO
    {
        [Required]
        public int UserId { get; set; }
    }
}
