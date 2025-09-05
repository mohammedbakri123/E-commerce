using E_commerce_Endpoints.DTO.Stock.Request;
using E_commerce_Endpoints.DTO.Stock.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IStockService
    {
        Task<ServiceResult<StockDTO>> Add(AddStockDTO dto);
        Task<ServiceResult<StockDTO>> Update(UpdateStockDTO dto);
        Task<ServiceResult<StockDTO>> GetByID(int id);
        Task<ServiceResult<IEnumerable<StockDTO>>> GetAll(int? variantId = null, int? supplierId = null, bool? isDone = null);
        Task<ServiceResult<bool>> Delete(int id);
    }
}
