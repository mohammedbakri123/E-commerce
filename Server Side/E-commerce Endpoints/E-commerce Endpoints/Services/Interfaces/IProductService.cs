using E_commerce_Endpoints.DTO.Product.Request;
using E_commerce_Endpoints.DTO.Product.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDTO>> Add(AddProductDTO productDTO);

        Task<ServiceResult<ProductDTO>> Update(UpdateProductDTO productDTO);

        Task<ServiceResult<ProductDTO>> GetByID(int id);

        Task<ServiceResult<ProductDTO>> GetByName(string name);

        Task<ServiceResult<IEnumerable<ProductDTO>>> GetAll(
    int? brandId = null,
    int? categoryId = null,
    int? subCategoryId = null,
    string? search = null
);


        Task<ServiceResult<bool>> Delete(int id);
    }
}
