using E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.DTO.Order.Request.E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.DTO.Order.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDTO>> Add(AddOrderDTO dto);
        Task<ServiceResult<OrderDTO>> Update(UpdateOrderDTO dto);
        Task<ServiceResult<OrderDTO>> GetById(int orderId);
        Task<ServiceResult<IEnumerable<OrderDTO>>> GetAll();
        Task<ServiceResult<bool>> Delete(int orderId);
        Task<ServiceResult<OrderDTO>> ChangeStatus(ChangeOrderStatusDTO dto);
    }
}
