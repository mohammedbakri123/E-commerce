using E_commerce_Endpoints.DTO.Admin.Request;
using E_commerce_Endpoints.DTO.Admin.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResult<AdminDTO>> Add(AddAdminDTO dto);
        Task<ServiceResult<AdminDTO>> Update(UpdateAdminDTO dto);
        Task<ServiceResult<AdminDTO>> GetByID(int id);
        Task<ServiceResult<IEnumerable<AdminDTO>>> GetAll();
        Task<ServiceResult<bool>> Delete(int id);
    }
}