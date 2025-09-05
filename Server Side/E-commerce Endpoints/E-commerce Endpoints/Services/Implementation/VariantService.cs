using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Variant.Request;
using E_commerce_Endpoints.DTO.Variant.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Services.Interfaces.E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class VariantService : IVariantService
    {
        private readonly appDbContext _context;
        private readonly ILogger<VariantService> _logger;

        public VariantService(appDbContext context, ILogger<VariantService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<VariantDTO>> Add(AddVariantDTO variantDTO)
        {
            try
            {
                if (!Validation.TryValidate(variantDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid Input: {messages}");
                    return ServiceResult<VariantDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var variant = new Variant
                {
                    ImagePaths = variantDTO.ImagePaths,
                    VariantProperties = variantDTO.VariantProperties,
                    Status = variantDTO.Status,
                    ProductId = variantDTO.ProductId
                };

                _context.Variants.Add(variant);
                await _context.SaveChangesAsync();

                return await GetByID(variant.VariantId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add variant: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<VariantDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add variant.");
            }
        }

        public async Task<ServiceResult<VariantDTO>> Update(UpdateVariantDTO variantDTO)
        {
            try
            {
                if (!Validation.TryValidate(variantDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid Input: {messages}");
                    return ServiceResult<VariantDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var variant = await _context.Variants.FindAsync(variantDTO.VariantId);
                if (variant == null)
                {
                    _logger.LogWarning($"Variant with ID {variantDTO.VariantId} not found.");
                    return ServiceResult<VariantDTO>.Fail(ServiceErrorType.NotFound, $"Variant with ID {variantDTO.VariantId} not found.");
                }

                variant.ImagePaths = variantDTO.ImagePaths;
                variant.VariantProperties = variantDTO.VariantProperties;
                variant.Status = variantDTO.Status;

                await _context.SaveChangesAsync();

                return await GetByID(variant.VariantId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update variant: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<VariantDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update variant.");
            }
        }

        public async Task<ServiceResult<VariantDTO>> GetByID(int id)
        {
            try
            {
                var variant = await _context.Variants
                    .Include(v => v.Product)
                    .FirstOrDefaultAsync(v => v.VariantId == id);

                if (variant == null)
                    return ServiceResult<VariantDTO>.Fail(ServiceErrorType.NotFound, $"Variant with ID {id} not found.");

                var response = new VariantDTO
                {
                    VariantId = variant.VariantId,
                    ImagePaths = variant.ImagePaths,
                    VariantProperties = variant.VariantProperties,
                    Status = variant.Status,
                    ProductId = variant.ProductId,
                    ProductName = variant.Product?.ProductName
                };

                return ServiceResult<VariantDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get variant by ID: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<VariantDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get variant.");
            }
        }

        public async Task<ServiceResult<IEnumerable<VariantDTO>>> GetAll(int? productId = null, bool? status = null)
        {
            try
            {
                var query = _context.Variants.AsQueryable();

                if (productId.HasValue)
                    query = query.Where(v => v.ProductId == productId.Value);

                if (status.HasValue)
                    query = query.Where(v => v.Status == status.Value);

                var variants = await query
                    .Include(v => v.Product)
                    .ToListAsync();

                var response = variants.Select(v => new VariantDTO
                {
                    VariantId = v.VariantId,
                    ImagePaths = v.ImagePaths,
                    VariantProperties = v.VariantProperties,
                    Status = v.Status,
                    ProductId = v.ProductId,
                    ProductName = v.Product?.ProductName
                });

                return ServiceResult<IEnumerable<VariantDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get all variants: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<IEnumerable<VariantDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get variants.");
            }
        }


        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var variant = await _context.Variants.FindAsync(id);
                if (variant == null)
                {
                    _logger.LogWarning($"Variant with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Variant with ID {id} not found.");
                }

                _context.Variants.Remove(variant);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete variant: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete variant.");
            }
        }
    }
}

