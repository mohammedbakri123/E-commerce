using E_commerce_Endpoints.DTO.Supplier.Request;
using E_commerce_Endpoints.DTO.Supplier.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<ServiceResult<SupplierDTO>> Add(AddSupplierDTO dto);
        Task<ServiceResult<SupplierDTO>> Update(UpdateSupplierDTO dto);
        Task<ServiceResult<SupplierDTO>> GetByID(int id);
        Task<ServiceResult<IEnumerable<SupplierDTO>>> GetAll();
        Task<ServiceResult<bool>> Delete(int id);
    }
}
