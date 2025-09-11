using E_commerce_Endpoints.DTO.Cart.Request;
using E_commerce_Endpoints.DTO.Cart.Response;
using E_commerce_Endpoints.DTO.Order.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
   
    public interface ICartService
    {
        Task<ServiceResult<int>> CreateCart(AddCartIDTO dto);
        Task<ServiceResult<CartDTO>> GetById(int cartId);
        Task<ServiceResult<CartDTO>> GetLastCart(int UserID);
        Task<ServiceResult<IEnumerable<CartDTO>>> GetAll(int? userId = null);
    }
}
