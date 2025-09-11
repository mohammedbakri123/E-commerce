using E_commerce_Endpoints.DTO.CartItem.Response;
namespace E_commerce_Endpoints.DTO.Cart.Response
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CartItemDTO>? Items { get; set; } = new();
        public decimal? TotalPrice => Items?.Sum(i => i.TotalPrice);

        public bool IsCLosed { get; set; }
    }
}
