using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Order.Request
{
    namespace E_commerce_Endpoints.DTO.Order.Request
    {
        public class UpdateOrderDTO
        {
            [Required]
            public int OrderId { get; set; }
            public string? PaymentMethod { get; set; }
            public int? DeliveryInfoId { get; set; }
        }
    }
}
