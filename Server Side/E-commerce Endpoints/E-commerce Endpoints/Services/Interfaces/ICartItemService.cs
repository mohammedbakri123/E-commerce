

using E_commerce_Endpoints.DTO.CartItem.Request;
using E_commerce_Endpoints.DTO.CartItem.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    
    public interface ICartItemService
    {
        Task<ServiceResult<CartItemDTO>> AddItem(AddCartItemDTO dto);
        Task<ServiceResult<CartItemDTO>> UpdateItem(UpdateCartItemDTO dto);
        Task<ServiceResult<bool>> DeleteItem(int cartItemId);

        Task<ServiceResult<IEnumerable<CartItemDTO>>> GetCartItems(int CartID);
    }
}
