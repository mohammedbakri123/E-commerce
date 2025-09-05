using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Offer.Request;
using E_commerce_Endpoints.DTO.Offer.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class OfferService : IOfferService
    {
        private readonly appDbContext _context;
        private readonly ILogger<OfferService> _logger;

        public OfferService(appDbContext context, ILogger<OfferService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<OfferDTO>> Add(AddOfferDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input while adding offer: {messages}");
                    return ServiceResult<OfferDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var variant = await _context.Variants.FindAsync(dto.VariantId);
                if (variant == null)
                {
                    _logger.LogWarning($"Attempted to add offer but variant {dto.VariantId} not found.");
                    return ServiceResult<OfferDTO>.Fail(ServiceErrorType.NotFound, $"Variant {dto.VariantId} not found.");
                }

                var offer = new Offer
                {
                    VariantId = dto.VariantId,
                    OfferPercentage = dto.OfferPercentage,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                _context.Offers.Add(offer);
                await _context.SaveChangesAsync();

                return await GetByID(offer.OfferId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add offer: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<OfferDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add offer.");
            }
        }

        public async Task<ServiceResult<OfferDTO>> Update(UpdateOfferDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input while updating offer: {messages}");
                    return ServiceResult<OfferDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var offer = await _context.Offers.FindAsync(dto.OfferId);
                if (offer == null)
                {
                    _logger.LogWarning($"Offer with ID {dto.OfferId} not found.");
                    return ServiceResult<OfferDTO>.Fail(ServiceErrorType.NotFound, $"Offer {dto.OfferId} not found.");
                }

                offer.VariantId = dto.VariantId;
                offer.OfferPercentage = dto.OfferPercentage;
                offer.StartDate = dto.StartDate;
                offer.EndDate = dto.EndDate;

                await _context.SaveChangesAsync();
                return await GetByID(offer.OfferId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update offer: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<OfferDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update offer.");
            }
        }

        public async Task<ServiceResult<OfferDTO>> GetByID(int id)
        {
            try
            {
                var offer = await _context.Offers
                    .Include(o => o.Variant)
                    .FirstOrDefaultAsync(o => o.OfferId == id);

                if (offer == null)
                {
                    _logger.LogWarning($"Offer {id} not found.");
                    return ServiceResult<OfferDTO>.Fail(ServiceErrorType.NotFound, $"Offer {id} not found.");
                }
                var response = new OfferDTO
                {
                    OfferId = offer.OfferId,
                    VariantId = offer.VariantId,
                    VariantName = offer.Variant?.VariantProperties,
                    OfferPercentage = offer.OfferPercentage,
                    StartDate = offer.StartDate,
                    EndDate = offer.EndDate
                };

                return ServiceResult<OfferDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get offer by ID: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<OfferDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get offer.");
            }
        }

        public async Task<ServiceResult<IEnumerable<OfferDTO>>> GetAll(int? variantId = null, bool? isActive = null)

        {
            try
            {
                var query = _context.Offers
                    .Include(o => o.Variant)
                    .AsQueryable();

                if (variantId.HasValue)
                    query = query.Where(o => o.VariantId == variantId.Value);

                if (isActive.HasValue)
                {
                    var now = DateTime.UtcNow;
                    if (isActive.Value)
                        query = query.Where(o => o.StartDate <= now && o.EndDate >= now);
                    else
                        query = query.Where(o => o.EndDate < now || o.StartDate > now);
                }

                var offers = await query.ToListAsync();

                var response = offers.Select(o => new OfferDTO
                {
                    OfferId = o.OfferId,
                    VariantId = o.VariantId,
                    VariantName = o.Variant?.VariantProperties,
                    OfferPercentage = o.OfferPercentage,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate
                });

                return ServiceResult<IEnumerable<OfferDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get offers: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<IEnumerable<OfferDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get offers.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var offer = await _context.Offers.FindAsync(id);
                if (offer == null)
                {
                    _logger.LogWarning($"Offer {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Offer {id} not found.");
                }

                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete offer: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete offer.");
            }
        }
    }
}
