namespace E_commerce_Endpoints.DTO.Product.Request
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? BrandId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
