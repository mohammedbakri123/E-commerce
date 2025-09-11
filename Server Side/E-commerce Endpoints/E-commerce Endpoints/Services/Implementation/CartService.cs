using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Cart.Request;
using E_commerce_Endpoints.DTO.Cart.Response;
using E_commerce_Endpoints.DTO.CartItem.Response;
using E_commerce_Endpoints.DTO.Order.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

public class CartService : ICartService
{
    private readonly appDbContext _context;
    private readonly ILogger<CartService> _logger;

    public CartService(appDbContext context, ILogger<CartService> logger)
    {
        _context = context;
        _logger = logger;
    }

 

    public  async Task<ServiceResult<int>> CreateCart(AddCartIDTO dto)
    {
        try
        {
            var cart = new Cart
            {
                UserId = dto.UserId,
                CreateAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                isClosed = false,
            };

            await _context.Carts.AddAsync(cart);


            await _context.SaveChangesAsync();

            // Now cart.Id is populated
            return ServiceResult<int>.Ok(cart.CartId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create new cart EX : {ex.Message} , at {DateTime.UtcNow}");
            return ServiceResult<int>.Fail(ServiceErrorType.ServerError, "Failed to create new cart");
        }
    }

    public async Task<ServiceResult<IEnumerable<CartDTO>>> GetAll(int? userId = null)
    {
        try
        {
            IQueryable<Cart> query = _context.Carts;

            if (userId.HasValue)
            {
                query = query.Where(c => c.UserId == userId.Value);
            }

            var carts = await query
                .OrderByDescending(c => c.CreateAt)
                .Select(c => new CartDTO
                {
                    CartId = c.CartId,
                    UserId = c.UserId,
                    CreatedAt = c.CreateAt,
                    UpdatedAt = c.UpdatedAt,
                    IsCLosed = c.isClosed,
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<CartDTO>>.Ok(carts);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed Get All Carts EX : {ex.Message} , at {DateTime.UtcNow}");
            return ServiceResult<IEnumerable<CartDTO>>.Fail(ServiceErrorType.ServerError, "Failed to get carts");
        }
    }

    public async Task<ServiceResult<CartDTO>> GetById(int cartId)
    {
        try
        {
            var cart = await _context.Carts
                .Where(c => c.CartId == cartId)
                .Select(c => new CartDTO
                {
                    CartId = c.CartId,
                    UserId = c.UserId,
                    CreatedAt = c.CreateAt,
                    UpdatedAt = c.UpdatedAt,
                    IsCLosed = c.isClosed,

                    Items = c.CartItems.Select(item => new CartItemDTO
                    {
                        CartItemId = item.CartItemId,
                        CartID = item.CartId,
                        VariantId = item.VariantId,
                        Quantity = item.Quantity,
                        PricePerUnit = item.PricePerUnit,
                        TotalPrice = item.TotalPrice ?? (item.PricePerUnit * item.Quantity)
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                _logger.LogWarning("Cart not found");
                return ServiceResult<CartDTO>.Fail(ServiceErrorType.NotFound, $"Cart with ID : {cartId} not found");
            }

            return ServiceResult<CartDTO>.Ok(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get cart by id, Ex : {ex.Message}, at {DateTime.UtcNow}");
            return ServiceResult<CartDTO>.Fail(ServiceErrorType.ServerError, "Failed to get cart by id");
        }
    }


    public async Task<ServiceResult<CartDTO>> GetLastCart(int userId)
    {
        try
        {
            // Try to get the last open cart
            var cart = await _context.Carts
                .Where(c => c.UserId == userId && !c.isClosed)
                .OrderByDescending(c => c.CreateAt)
                .Select(c => new CartDTO
                {
                    CartId = c.CartId,
                    UserId = c.UserId,
                    CreatedAt = c.CreateAt,
                    UpdatedAt = c.UpdatedAt,
                    IsCLosed = c.isClosed
                })
                .FirstOrDefaultAsync();

            // If no open cart found, create a new one
            if (cart == null)
            {
                var createResult = await CreateCart(new AddCartIDTO { UserId = userId });
                if (!createResult.Success)
                {
                    _logger.LogWarning($"Failed to Get or create new cart for this user  UserID : {userId}");
                    return ServiceResult<CartDTO>.Fail(ServiceErrorType.ServerError, $"Failed to Get or create new cart for this user  UserID : {userId}");
                }

                // Return the newly created cart as CartDTO
                cart = new CartDTO
                {
                    CartId = createResult.Data,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsCLosed = false
                };
            }

            return ServiceResult<CartDTO>.Ok(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get last cart for user, Ex : {ex.Message} , at : {DateTime.UtcNow}");
            return ServiceResult<CartDTO>.Fail(ServiceErrorType.ServerError, "Failed to get last cart for user");
        }
    }

}
