namespace E_commerce_Endpoints.DTO.Variant.Response
{
    public class VariantDTO
    {
        public int VariantId { get; set; }
        public string? ImagePaths { get; set; }
        public string? VariantProperties { get; set; }
        public bool? Status { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
    }
}
