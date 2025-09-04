namespace E_commerce_Endpoints.DTO.Product.Request
{
    public class AddProductDTO
    {
        public string Name { get; set; } = null!;
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
