using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IBrandService
    {
        public Task<ServiceResult<BrandDTO>> Add(AddBrandDTO BrandDTO);

        public Task<ServiceResult<BrandDTO>> Update(UpdateBrandDTO BrandDTO);

        public Task<ServiceResult<BrandDTO>> GetByID(int Id);
        public Task<ServiceResult<BrandDTO>> GetByName(string name);
        public Task<ServiceResult<IEnumerable<BrandDTO>>> GetAll();

        public Task<ServiceResult<bool>> Delete(int id);
    }
}
