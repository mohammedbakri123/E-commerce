using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.CartItem.Request;
using E_commerce_Endpoints.DTO.CartItem.Response;
using E_commerce_Endpoints.Services.Implementation;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementations
{
    public class CartItemService : ICartItemService
    {
        private readonly appDbContext _context;
        private readonly ILogger<CartItemService> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public CartItemService(appDbContext context, ILogger<CartItemService> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public async Task<ServiceResult<CartItemDTO>> AddItem(AddCartItemDTO dto)
        {
            try
            {
                var cart = await _context.Carts.FindAsync(dto.CartId);
                if (cart == null)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.NotFound, $"Cart {dto.CartId} not found");
                }

                var variant = await _context.Variants.FindAsync(dto.VariantId);
                if (variant == null)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.NotFound, $"Variant {dto.VariantId} not found");
                }
                var variantLogger = _loggerFactory.CreateLogger<VariantService>();
                var variantService = new VariantService(_context, variantLogger);

                var VariantWithPriceResult = await variantService.GetByIDWithPriceAndQuantity(dto.VariantId);
                if (!VariantWithPriceResult.Success || VariantWithPriceResult == null)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.NotFound, $"Failed To Get Variant {dto.VariantId} Price or Quantity");
                }

                if(VariantWithPriceResult.Data.Quantity < dto.Quantity)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.ServerError, $"Failed To Add Variant : {dto.VariantId}, Quantity is not enough");
                }

                var cartItem = new CartItem
                {
                    CartId = dto.CartId,
                    VariantId = dto.VariantId,
                    Quantity = dto.Quantity,
                    PricePerUnit = VariantWithPriceResult.Data.Price ?? 0,
                    TotalPrice = VariantWithPriceResult.Data.Price * dto.Quantity
                };

                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();

                var response = new CartItemDTO
                {
                    CartItemId = cartItem.CartItemId,
                    CartID = cartItem.CartId,
                    VariantId = cartItem.VariantId,
                    Quantity = cartItem.Quantity,
                    PricePerUnit = cartItem.PricePerUnit,
                    TotalPrice = cartItem.TotalPrice ?? 0
                };

                return ServiceResult<CartItemDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add item: {ex.Message}");
                return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.ServerError, "Failed to add item to cart");
            }
        }

        public async Task<ServiceResult<CartItemDTO>> UpdateItem(UpdateCartItemDTO dto)
        {
            try
            {
                var cartItem = await _context.CartItems.FindAsync(dto.CartItemId);
                if (cartItem == null)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.NotFound, $"CartItem {dto.CartItemId} not found");
                }

                var variantLogger = _loggerFactory.CreateLogger<VariantService>();
                var variantService = new VariantService(_context, variantLogger);

                var VariantWithPriceResult = await variantService.GetByIDWithPriceAndQuantity(cartItem.VariantId);
                if (!VariantWithPriceResult.Success || VariantWithPriceResult == null)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.NotFound, $"Failed To Get Variant {cartItem.VariantId} Price or Quantity");
                }

                if (VariantWithPriceResult.Data.Quantity < dto.Quantity)
                {
                    return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.ServerError, $"Failed To Update Variant : {VariantWithPriceResult.Data.Price}, Quantity is not enough");
                }

                cartItem.Quantity = dto.Quantity;
                cartItem.PricePerUnit = cartItem.PricePerUnit;
                cartItem.TotalPrice = cartItem.PricePerUnit * dto.Quantity;

                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();

                var response = new CartItemDTO
                {
                    CartItemId = cartItem.CartItemId,
                    CartID = cartItem.CartId,
                    VariantId = cartItem.VariantId,
                    Quantity = cartItem.Quantity,
                    PricePerUnit = cartItem.PricePerUnit,
                    TotalPrice = cartItem.TotalPrice
                };

                return ServiceResult<CartItemDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update item: {ex.Message}");
                return ServiceResult<CartItemDTO>.Fail(ServiceErrorType.ServerError, "Failed to update cart item");
            }
        }

        public async Task<ServiceResult<bool>> DeleteItem(int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems.FindAsync(cartItemId);
                if (cartItem == null)
                {
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"CartItem {cartItemId} not found");
                }

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete item: {ex.Message}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Failed to delete cart item");
            }
        }

        public async Task<ServiceResult<IEnumerable<CartItemDTO>>> GetCartItems(int cartId)
        {
            try
            {
                var items = await _context.CartItems
                    .Where(ci => ci.CartId == cartId)
                    .Select(ci => new CartItemDTO
                    {
                        CartItemId = ci.CartItemId,
                        CartID = ci.CartId,
                        VariantId = ci.VariantId,
                        Quantity = ci.Quantity,
                        PricePerUnit = ci.PricePerUnit,
                        TotalPrice = ci.TotalPrice ?? (ci.PricePerUnit * ci.Quantity)
                    })
                    .ToListAsync();

                return ServiceResult<IEnumerable<CartItemDTO>>.Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get items for cart {cartId}: {ex.Message}");
                return ServiceResult<IEnumerable<CartItemDTO>>.Fail(ServiceErrorType.ServerError, "Failed to get cart items");
            }
        }
    }
}
