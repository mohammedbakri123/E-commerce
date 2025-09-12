namespace E_commerce_Endpoints.DTO.Order.Request
{
    public class ChangeOrderStatusDTO
    {
        public int OrderId { get; set; }
        public string? status { get; set; }

    }
}
