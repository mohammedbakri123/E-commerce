using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface ISubCategoryService
    {
        public Task<ServiceResult<SubCategoryDTO>> Add(AddSubCategoryDTO SubcategoryDTO);

        public Task<ServiceResult<SubCategoryDTO>> Update(UpdateSubCategoryDTO SubcategoryDTO);

        public Task<ServiceResult<SubCategoryDTO>> GetByID(int Id);
        public Task<ServiceResult<SubCategoryDTO>> GetByName(string name);
        public Task<ServiceResult<IEnumerable<SubCategoryDTO>>> GetAll();

        public Task<ServiceResult<bool>> Delete(int id);
    }
}
