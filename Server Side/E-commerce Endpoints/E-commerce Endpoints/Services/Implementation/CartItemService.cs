using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Cart.Response;
using E_commerce_Endpoints.DTO.CartItem.Request;
using E_commerce_Endpoints.DTO.CartItem.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

public class CartItemService : ICartItemService
{
    public Task<ServiceResult<CartItemDTO>> AddItem(AddCartItemDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<bool>> DeleteItem(int cartItemId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<CartItemDTO>> UpdateItem(UpdateCartItemDTO dto)
    {
        throw new NotImplementedException();
    }
}
