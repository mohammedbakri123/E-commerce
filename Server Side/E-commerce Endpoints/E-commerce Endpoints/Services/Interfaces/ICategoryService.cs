using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<ServiceResult<CategoryDTO>>Add(AddCategoryDTO categoryDTO);

        public Task<ServiceResult<CategoryDTO>> Update(UpdateCategoryDTO categoryDTO);

        public Task<ServiceResult<CategoryDTO>> GetByID(int Id);
        public Task<ServiceResult<CategoryDTO>> GetByName(string name);
        public Task<ServiceResult<IEnumerable<CategoryDTO>>> GetAll();

        public Task<ServiceResult<bool>> Delete(int id);

    }
}
