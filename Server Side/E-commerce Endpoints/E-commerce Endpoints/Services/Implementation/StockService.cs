using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Stock.Request;
using E_commerce_Endpoints.DTO.Stock.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class StockService : IStockService
    {
        private readonly appDbContext _context;
        private readonly ILogger<StockService> _logger;

        public StockService(appDbContext context, ILogger<StockService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<StockDTO>> Add(AddStockDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid stock input: {messages}");
                    return ServiceResult<StockDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                // Validate Variant
                var variant = await _context.Variants.FindAsync(dto.VariantId);
                if (variant == null)
                {
                    _logger.LogWarning($"Variant {dto.VariantId} not found.");
                    return ServiceResult<StockDTO>.Fail(ServiceErrorType.NotFound, $"Variant {dto.VariantId} not found.");
                }

                // Validate Supplier if provided
                Supplier? supplier = null;
                if (dto.SupplierId.HasValue)
                {
                    supplier = await _context.Suppliers.FindAsync(dto.SupplierId.Value);
                    if (supplier == null)
                    {
                        _logger.LogWarning($"Supplier {dto.SupplierId.Value} not found.");
                        return ServiceResult<StockDTO>.Fail(ServiceErrorType.NotFound, $"Supplier {dto.SupplierId.Value} not found.");
                    }
                }

                var stock = new Stock
                {
                    VariantId = dto.VariantId,
                    EntranceQuantity = dto.EntranceQuantity,
                    CurrentQuantity = dto.CurrentQuantity ?? dto.EntranceQuantity, // Default current = entrance
                    EntranceDate = dto.EntranceDate ?? DateTime.UtcNow,
                    ExpireDate = dto.ExpireDate,
                    CostPrice = dto.CostPrice,
                    SellPrice = dto.SellPrice,
                    SupplierId = dto.SupplierId,
                    IsDone = false
                };

                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();

                return await GetByID(stock.StockId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add stock: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<StockDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add stock.");
            }
        }

        public async Task<ServiceResult<StockDTO>> Update(UpdateStockDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid stock update: {messages}");
                    return ServiceResult<StockDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var stock = await _context.Stocks
                    .Include(s => s.Supplier)
                    .Include(s => s.Variant)
                    .FirstOrDefaultAsync(s => s.StockId == dto.StockId);

                if (stock == null)
                {
                    _logger.LogWarning($"Stock {dto.StockId} not found.");
                    return ServiceResult<StockDTO>.Fail(ServiceErrorType.NotFound, $"Stock {dto.StockId} not found.");
                }

                if (dto.SupplierId.HasValue)
                {
                    var supplier = await _context.Suppliers.FindAsync(dto.SupplierId.Value);
                    if (supplier == null)
                    {
                        _logger.LogWarning($"Supplier {dto.SupplierId.Value} not found.");
                        return ServiceResult<StockDTO>.Fail(ServiceErrorType.NotFound, $"Supplier {dto.SupplierId.Value} not found.");
                    }
                }

                stock.EntranceQuantity = dto.EntranceQuantity;
                stock.CurrentQuantity = dto.CurrentQuantity;
                stock.EntranceDate = dto.EntranceDate ?? stock.EntranceDate;
                stock.ExpireDate = dto.ExpireDate;
                stock.CostPrice = dto.CostPrice;
                stock.SellPrice = dto.SellPrice;
                stock.SupplierId = dto.SupplierId;

                await _context.SaveChangesAsync();

                return await GetByID(stock.StockId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update stock: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<StockDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update stock.");
            }
        }

        public async Task<ServiceResult<StockDTO>> GetByID(int id)
        {
            try
            {
                var stock = await _context.Stocks
                    .Include(s => s.Variant)
                    .Include(s => s.Supplier)
                    .FirstOrDefaultAsync(s => s.StockId == id);

                if (stock == null)
                    return ServiceResult<StockDTO>.Fail(ServiceErrorType.NotFound, $"Stock {id} not found.");

                var response = new StockDTO
                {
                    StockId = stock.StockId,
                    VariantId = stock.VariantId,
                    VariantName = stock.Variant?.VariantProperties,
                    EntranceQuantity = stock.EntranceQuantity,
                    CurrentQuantity = stock.CurrentQuantity,
                    EntranceDate = stock.EntranceDate,
                    ExpireDate = stock.ExpireDate,
                    CostPrice = stock.CostPrice,
                    SellPrice = stock.SellPrice,
                    SupplierId = stock.SupplierId,
                    SupplierName = stock.Supplier?.CompanyName,
                    IsDone = stock.IsDone ?? false
                };

                return ServiceResult<StockDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get stock by ID: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<StockDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get stock.");
            }
        }

        public async Task<ServiceResult<IEnumerable<StockDTO>>> GetAll(int? variantId = null, int? supplierId = null, bool? isDone = null)
        {
            try
            {
                var query = _context.Stocks
                    .Include(s => s.Variant)
                    .Include(s => s.Supplier)
                    .AsQueryable();

                if (variantId.HasValue)
                    query = query.Where(s => s.VariantId == variantId.Value);

                if (supplierId.HasValue)
                    query = query.Where(s => s.SupplierId == supplierId.Value);

                if (isDone.HasValue)
                    query = query.Where(s => s.IsDone == isDone.Value);

                var stocks = await query.ToListAsync();

                var response = stocks.Select(stock => new StockDTO
                {
                    StockId = stock.StockId,
                    VariantId = stock.VariantId,
                    VariantName = stock.Variant?.VariantProperties,
                    EntranceQuantity = stock.EntranceQuantity,
                    CurrentQuantity = stock.CurrentQuantity,
                    EntranceDate = stock.EntranceDate,
                    ExpireDate = stock.ExpireDate,
                    CostPrice = stock.CostPrice,
                    SellPrice = stock.SellPrice,
                    SupplierId = stock.SupplierId,
                    SupplierName = stock.Supplier?.CompanyName,
                    IsDone = stock.IsDone ?? false
                });

                return ServiceResult<IEnumerable<StockDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get all stocks: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<IEnumerable<StockDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get stocks.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var stock = await _context.Stocks.FindAsync(id);
                if (stock == null)
                {
                    _logger.LogWarning($"Stock {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Stock {id} not found.");
                }

                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete stock: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete stock.");
            }
        }
    }
}
