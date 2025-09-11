using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class BrandService : IBrandService
    {
        private readonly appDbContext _context;
        private readonly ILogger<BrandService> _logger;

        public BrandService(appDbContext context, ILogger<BrandService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<BrandDTO>> Add(AddBrandDTO brandDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(brandDTO.Name))
                {
                    _logger.LogWarning("Tried to add a brand with empty name.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Validation, "Brand name cannot be empty.");
                }

                if (await _context.Brands.AnyAsync(b => b.BrandName == brandDTO.Name))
                {
                    _logger.LogWarning($"Duplicate brand creation attempt: {brandDTO.Name}");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Duplicate, "Brand already exists.");
                }

                var brand = new Brand
                {
                    BrandName = brandDTO.Name
                };

                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();

                var response = new BrandDTO
                {
                    Id = brand.BrandId,
                    Name = brand.BrandName
                };

                return ServiceResult<BrandDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add brand: {ex.Message}");
                return ServiceResult<BrandDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to add brand. {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid brand ID format: {id}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Invalid brand ID format.");
                }

                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    _logger.LogWarning($"Brand with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Brand with ID {id} not found.");
                }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Brand with ID {id} deleted successfully.");
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete brand: {ex.Message}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, $"Server failed to delete brand.  {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<BrandDTO>>> GetAll()
        {
            try
            {
                var brands = await _context.Brands.ToListAsync();
                var response = brands.Select(b => new BrandDTO
                {
                    Id = b.BrandId,
                    Name = b.BrandName
                });

                return ServiceResult<IEnumerable<BrandDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve brands: {ex.Message}");
                return ServiceResult<IEnumerable<BrandDTO>>.Fail(ServiceErrorType.ServerError, $"Server failed to retrieve brands.  {ex.Message}");
            }
        }

        public async Task<ServiceResult<BrandDTO>> GetByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid brand ID format: {id}");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Validation, "Invalid brand ID format.");
                }

                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    _logger.LogWarning($"Brand with ID {id} not found.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.NotFound, $"Brand with ID {id} not found.");
                }

                var response = new BrandDTO
                {
                    Id = brand.BrandId,
                    Name = brand.BrandName
                };

                return ServiceResult<BrandDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve brand: {ex.Message}");
                return ServiceResult<BrandDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to retrieve brand.  {ex.Message}");
            }
        }

        public async Task<ServiceResult<BrandDTO>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("Tried to search brand with empty name.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Validation, "Brand name cannot be empty.");
                }

                var brand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == name);
                if (brand == null)
                {
                    _logger.LogWarning($"Brand with name {name} not found.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.NotFound, $"Brand with name {name} not found.");
                }

                var response = new BrandDTO
                {
                    Id = brand.BrandId,
                    Name = brand.BrandName
                };

                return ServiceResult<BrandDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve brand by name: {ex.Message}");
                return ServiceResult<BrandDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to retrieve brand.  {ex.Message}");
            }
        }

        public async Task<ServiceResult<BrandDTO>> Update(UpdateBrandDTO brandDTO)
        {
            try
            {
                if (brandDTO.id <= 0)
                {
                    _logger.LogWarning($"Invalid brand ID format: {brandDTO.id}");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Validation, "Invalid brand ID format.");
                }
                if (string.IsNullOrWhiteSpace(brandDTO.Name))
                {
                    _logger.LogWarning("Tried to update brand with empty name.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Validation, "Brand name cannot be empty.");
                }

                if (await _context.Brands.AnyAsync(b => b.BrandName == brandDTO.Name))
                {
                    _logger.LogWarning($"Duplicate brand Update attempt: {brandDTO.Name}");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.Duplicate, $"Brand Name : {brandDTO.Name} already exists.");
                }

                var brand = await _context.Brands.FindAsync(brandDTO.id);
                if (brand == null)
                {
                    _logger.LogWarning($"Brand with ID {brandDTO.id} not found for update.");
                    return ServiceResult<BrandDTO>.Fail(ServiceErrorType.NotFound, $"Brand with ID {brandDTO.id} not found.");
                }

                brand.BrandName = brandDTO.Name;
                await _context.SaveChangesAsync();

                var response = new BrandDTO
                {
                    Id = brand.BrandId,
                    Name = brand.BrandName
                };

                _logger.LogInformation($"Brand with ID {brandDTO.id} updated successfully.");
                return ServiceResult<BrandDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update brand: {ex.Message}");
                return ServiceResult<BrandDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to update brand.  {ex.Message}");
            }
        }
    }
}
