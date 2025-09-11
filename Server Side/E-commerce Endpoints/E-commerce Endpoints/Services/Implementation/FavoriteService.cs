using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.DTO.Favorite.Request;
using E_commerce_Endpoints.DTO.Favorite.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;
using E_commerce_Endpoints.Helper;



    namespace E_commerce_Endpoints.Services.Implementation
    {
        public class FavoriteService : IFavoriteService
        {
            private readonly appDbContext _context;
            private readonly ILogger<FavoriteService> _logger;

            public FavoriteService(appDbContext context, ILogger<FavoriteService> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<ServiceResult<FavoriteDTO>> Add(AddFavoriteDTO dto)
            {
                try
                {
                    if (!Validation.TryValidate(dto, out var validationErrors))
                    {
                        var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                        _logger.LogWarning($"Invalid input: {messages}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                    }

                    // Check if user exists
                    var user = await _context.Users.FindAsync(dto.UserId);
                    if (user == null)
                    {
                        _logger.LogWarning($"User not found: {dto.UserId}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.NotFound, $"User with ID {dto.UserId} not found.");
                    }

                    // Check if variant exists
                    var variant = await _context.Variants.FindAsync(dto.VariantId);
                    if (variant == null)
                    {
                        _logger.LogWarning($"Variant not found: {dto.VariantId}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.NotFound, $"Variant with ID {dto.VariantId} not found.");
                    }

                    // Check if already favorite
                    bool exists = await _context.Favorites.AnyAsync(f => f.UserId == dto.UserId && f.VariantId == dto.VariantId);
                    if (exists)
                    {
                        _logger.LogWarning($"Favorite already exists for User {dto.UserId} and Variant {dto.VariantId}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.Duplicate, "This variant is already in favorites for this user.");
                    }

                    var favorite = new Favorite
                    {
                        UserId = dto.UserId,
                        VariantId = dto.VariantId,
                        CreateAt = DateTime.UtcNow
                    };

                    _context.Favorites.Add(favorite);
                    await _context.SaveChangesAsync();

                    return await GetByID(favorite.FavoriteId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Server failed to add favorite: {ex.Message}, at {DateTime.UtcNow}");
                    return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add favorite.");
                }
            }

            public async Task<ServiceResult<FavoriteDTO>> Update(UpdateFavoriteDTO dto)
            {
                try
                {
                    if (!Validation.TryValidate(dto, out var validationErrors))
                    {
                        var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                        _logger.LogWarning($"Invalid input: {messages}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                    }

                    var favorite = await _context.Favorites.FindAsync(dto.Id);
                    if (favorite == null)
                    {
                        _logger.LogWarning($"Favorite with ID {dto.Id} not found");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.NotFound, $"Favorite with ID {dto.Id} not found.");
                    }

                    // Check for duplicates
                    bool exists = await _context.Favorites.AnyAsync(f => f.UserId == dto.UserId && f.VariantId == dto.VariantId && f.FavoriteId != dto.Id);
                    if (exists)
                    {
                        _logger.LogWarning($"Duplicate favorite for User {dto.UserId} and Variant {dto.VariantId}");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.Duplicate, "This variant is already in favorites for this user.");
                    }

                    favorite.UserId = dto.UserId;
                    favorite.VariantId = dto.VariantId;

                    await _context.SaveChangesAsync();
                    return await GetByID(favorite.FavoriteId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Server failed to update favorite: {ex.Message}, at {DateTime.UtcNow}");
                    return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update favorite.");
                }
            }

            public async Task<ServiceResult<FavoriteDTO>> GetByID(int id)
            {
                try
                {
                    var favorite = await _context.Favorites
                        .Include(f => f.User)
                        .Include(f => f.Variant)
                        .ThenInclude(v => v.Product)
                        .FirstOrDefaultAsync(f => f.FavoriteId == id);

                    if (favorite == null)
                    {
                        _logger.LogWarning($"Favorite with ID {id} not found.");
                        return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.NotFound, $"Favorite with ID {id} not found.");
                    }
                    var response = new FavoriteDTO
                    {
                        Id = favorite.FavoriteId,
                        UserId = favorite.UserId,
                        UserName = favorite.User.UserFirstName,
                        VariantId = favorite.VariantId,
                        VariantName = favorite.Variant?.Product?.ProductName,
                        CreatedAt = favorite.CreateAt
                    };

                    return ServiceResult<FavoriteDTO>.Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Server failed to get favorite by ID: {ex.Message}, at {DateTime.UtcNow}");
                    return ServiceResult<FavoriteDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get favorite.");
                }
            }

            public async Task<ServiceResult<IEnumerable<FavoriteDTO>>> GetAll(int? userId = null, int? variantId = null)
            {
                try
                {
                    var query = _context.Favorites
                        .Include(f => f.User)
                        .Include(f => f.Variant)
                        .ThenInclude(v => v.Product)
                        .AsQueryable();

                    if (userId.HasValue)
                        query = query.Where(f => f.UserId == userId.Value);
                    if (variantId.HasValue)
                        query = query.Where(f => f.VariantId == variantId.Value);

                    var favorites = await query.ToListAsync();

                    var response = favorites.Select(f => new FavoriteDTO
                    {
                        Id = f.FavoriteId,
                        UserId = f.UserId,
                        UserName = f.User?.UserFirstName,
                        VariantId = f.VariantId,
                        VariantName = f.Variant?.Product?.ProductName,
                        CreatedAt = f.CreateAt
                    });

                    return ServiceResult<IEnumerable<FavoriteDTO>>.Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Server failed to get all favorites: {ex.Message}, at {DateTime.UtcNow}");
                    return ServiceResult<IEnumerable<FavoriteDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get favorites.");
                }
            }

            public async Task<ServiceResult<bool>> Delete(int id)
            {
                try
                {
                    var favorite = await _context.Favorites.FindAsync(id);
                    if (favorite == null)
                    {
                        _logger.LogWarning($"Favorite with ID {id} not found.");
                        return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Favorite with ID {id} not found.");
                    }

                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();

                    return ServiceResult<bool>.Ok(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Server failed to delete favorite: {ex.Message}, at {DateTime.UtcNow}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete favorite.");
                }
            }
        }
    }


