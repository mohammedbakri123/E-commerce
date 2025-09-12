using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Order.Request
{
    public class AddOrderDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        [Required]
        public int? DeliveryInfoId { get; set; }
        [Required]
        public int CartId { get; set; }
    }
}

