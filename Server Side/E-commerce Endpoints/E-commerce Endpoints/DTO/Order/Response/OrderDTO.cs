namespace E_commerce_Endpoints.DTO.Order.Response
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public int? DeliveryInfoId { get; set; }
        public int? CartId { get; set; }

        // Related info (optional for richer responses)
        public string? UserName { get; set; }
    }
}