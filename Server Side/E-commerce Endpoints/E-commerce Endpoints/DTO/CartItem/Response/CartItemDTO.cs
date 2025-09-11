namespace E_commerce_Endpoints.DTO.CartItem.Response
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int CartID { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; }
        public bool HasOffer { get; set; }
        public decimal? OfferPercentage { get; set; }
    }
}
