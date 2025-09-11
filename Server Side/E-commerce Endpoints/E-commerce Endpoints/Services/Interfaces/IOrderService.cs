using E_commerce_Endpoints.DTO.Order.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDTO>> CheckOut(int CartID, int DeliveryInfoID);

    }
}
