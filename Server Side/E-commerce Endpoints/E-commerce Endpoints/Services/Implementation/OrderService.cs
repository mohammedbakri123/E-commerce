using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Offer.Response;
using E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.DTO.Order.Request.E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.DTO.Order.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly appDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly ILoggerFactory _loggerFactory;


        public OrderService(appDbContext context, ILogger<OrderService> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public async Task<ServiceResult<OrderDTO>> Add(AddOrderDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input while adding Order: {messages}");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var IsUserExist = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
                if (!IsUserExist)
                {
                    _logger.LogWarning("Tried to add order to user not exist");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"User : {dto.UserId}, not found");
                }

                var IsDeliveryInfoIdExist = await _context.DeliveryInfos.AnyAsync(d => d.DeliveryInfoId == dto.DeliveryInfoId);
                if (!IsDeliveryInfoIdExist)
                {
                    _logger.LogWarning("Tried To Add Order with no Delivery info");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"You Must Add Valid Delivery Info");
                }

                // 📌 جلب عناصر السلة
                var cartItems = await _context.CartItems
                    .Where(ci => ci.CartId == dto.CartId)
                    .ToListAsync();

                if (cartItems == null || cartItems.Count == 0)
                {
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, "Cart is empty, cannot create order.");
                }

                decimal totalAmount = 0;

                foreach (var item in cartItems)
                {
                    var stock = await _context.Stocks
                        .Where(s => s.VariantId == item.VariantId && (s.IsDone == null || s.IsDone == false))
                        .OrderByDescending(s => s.EntranceDate)
                        .FirstOrDefaultAsync();

                    if (stock == null)
                    {
                        return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound,
                            $"No stock found for Variant {item.VariantId}");
                    }

                    if (stock.CurrentQuantity < item.Quantity)
                    {
                        return ServiceResult<OrderDTO>.Fail(ServiceErrorType.Validation,
                            $"Not enough stock for Variant {item.VariantId}. Available: {stock.CurrentQuantity}, Requested: {item.Quantity}");
                    }

                    decimal itemPrice = stock.SellPrice ?? 0;
                    totalAmount += item.Quantity * itemPrice;

                    stock.CurrentQuantity -= item.Quantity;

                    if (stock.CurrentQuantity == 0)
                    {
                        stock.IsDone = true;
                    }

                    _context.Stocks.Update(stock);
                }

                // 📌 إنشاء الطلب
                var order = new Order
                {
                    UserId = dto.UserId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = totalAmount,
                    PaymentMethod = dto.PaymentMethod,
                    DeliveryInfoId = dto.DeliveryInfoId,
                    CartId = dto.CartId
                };

                await _context.Orders.AddAsync(order);

                // 📌 إغلاق السلة بعد إنشاء الطلب
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == dto.CartId);
                if (cart == null)
                {
                    _logger.LogWarning($"Cart : {dto.CartId}, not found. Could not close the Cart");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"Cart : {dto.CartId}, not found. Could not close the Cart");

                }
                cart!.isClosed = true;
                _context.Carts.Update(cart);



                await _context.SaveChangesAsync();

                return ServiceResult<OrderDTO>.Ok(MapToDTO(order));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add order: {ex.Message}");
                return ServiceResult<OrderDTO>.Fail(ServiceErrorType.ServerError, "Failed to add order");
            }
        }





        public async Task<ServiceResult<OrderDTO>> Update(UpdateOrderDTO dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(dto.OrderId);
                if (order == null)
                {
                    _logger.LogWarning ($"Order {dto.OrderId} not found");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"Order {dto.OrderId} not found");

                }
                order.PaymentMethod = dto.PaymentMethod ?? order.PaymentMethod;
                order.DeliveryInfoId = dto.DeliveryInfoId ?? order.DeliveryInfoId;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return ServiceResult<OrderDTO>.Ok(MapToDTO(order));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update order: {ex.Message}");
                return ServiceResult<OrderDTO>.Fail(ServiceErrorType.ServerError, "Failed to update order");
            }
        }

        public async Task<ServiceResult<OrderDTO>> GetById(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.DeliveryInfo)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    _logger.LogWarning($"Order {orderId} not found");
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"Order {orderId} not found");

                }
                return ServiceResult<OrderDTO>.Ok(MapToDTO(order));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order {orderId}: {ex.Message}");
                return ServiceResult<OrderDTO>.Fail(ServiceErrorType.ServerError, "Failed to get order");
            }
        }
        public async Task<ServiceResult<IEnumerable<OrderDTO>>> GetAll()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.DeliveryInfo)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                if (orders == null || orders.Count == 0)
                {
                    return ServiceResult<IEnumerable<OrderDTO>>.Fail(ServiceErrorType.NotFound, "No orders in the database");
                }

                var dtos = orders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    UserName = o.User != null ? o.User.Email : "Unknown",  // Safe null check
                    OrderDate = o.OrderDate ?? null,
                    Status = o.Status ?? "Unknown",
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod ?? "Unknown",
                    DeliveryInfoId = o.DeliveryInfoId ?? null,
                    CartId = o.CartId
                }).ToList();

                return ServiceResult<IEnumerable<OrderDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders: {ex.Message}");
                return ServiceResult<IEnumerable<OrderDTO>>.Fail(ServiceErrorType.ServerError, $"Failed to get orders: {ex.Message}");
            }
        }




        public async Task<ServiceResult<bool>> Delete(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Order {orderId} not found");

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete order {orderId}: {ex.Message}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Failed to delete order");
            }
        }

        public async Task<ServiceResult<OrderDTO>> ChangeStatus(ChangeOrderStatusDTO dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(dto.OrderId);
                if (order == null)
                {
                    return ServiceResult<OrderDTO>.Fail(ServiceErrorType.NotFound, $"Order {dto.OrderId} not found");
                }

                // تحديث الحالة
                order.Status = string.IsNullOrWhiteSpace(dto.status) ? order.Status : dto.status;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return ServiceResult<OrderDTO>.Ok(MapToDTO(order));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to change order status {dto.OrderId}: {ex.Message}");
                return ServiceResult<OrderDTO>.Fail(ServiceErrorType.ServerError, "Failed to change order status");
            }
        }


        private OrderDTO MapToDTO(Order order)
        {
            return new OrderDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                DeliveryInfoId = order.DeliveryInfoId,
                CartId = order.CartId,
                UserName = order.User?.UserFirstName,

            };
        }
    }
}
