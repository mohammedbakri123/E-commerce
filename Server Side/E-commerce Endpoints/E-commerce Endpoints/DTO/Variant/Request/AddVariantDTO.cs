namespace E_commerce_Endpoints.DTO.Variant.Request
{
    public class AddVariantDTO
    {
        public string? ImagePaths { get; set; }
        public string? VariantProperties { get; set; }
        public bool? Status { get; set; }
        public string? VariantDesc { get; set; }
        public int ProductId { get; set; }
    }
}
