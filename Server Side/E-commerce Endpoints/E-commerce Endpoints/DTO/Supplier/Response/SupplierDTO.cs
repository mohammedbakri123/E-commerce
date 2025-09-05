namespace E_commerce_Endpoints.DTO.Supplier.Response
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
