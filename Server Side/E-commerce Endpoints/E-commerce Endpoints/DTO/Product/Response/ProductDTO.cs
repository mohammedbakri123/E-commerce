using E_commerce_Endpoints.DTO.Category.Response;

namespace E_commerce_Endpoints.DTO.Product.Response
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int BrandID { get; set; }
        public string? BrandName { get; set; }
        public int SubCategoryID { get; set; }
        public string? SubCategoryName { get; set; }
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get;set; }
    }
}
