using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly appDbContext _context;
        private readonly ILogger<SubCategoryService> _logger;

        public SubCategoryService(appDbContext context, ILogger<SubCategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<SubCategoryDTO>> Add(AddSubCategoryDTO subCategoryDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subCategoryDTO.Name))
                {
                    _logger.LogWarning("Tried to add a subcategory with empty name.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "SubCategory name cannot be empty.");
                }

                if (await _context.SubCategories.AnyAsync(s => s.SubcategoryName == subCategoryDTO.Name))
                {
                    _logger.LogWarning($"Duplicate subcategory creation attempt: {subCategoryDTO.Name}");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Duplicate, "SubCategory already exists.");
                }
                if (! await _context.Categories.AnyAsync(c => c.CategoryId == subCategoryDTO.CategoryID))
                {
                    _logger.LogWarning($"Category ID : {subCategoryDTO.CategoryID} is Not valied");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "CategoryID Dose Not exists.");
                }

                var subCategory = new SubCategory
                {
                    SubcategoryName = subCategoryDTO.Name,
                    CategoryId = subCategoryDTO.CategoryID
                };

                _context.SubCategories.Add(subCategory);
                await _context.SaveChangesAsync();

                var response = new SubCategoryDTO
                {
                    Id = subCategory.SubcategoryId,
                    Name = subCategory.SubcategoryName,
                    CategoryId = subCategory.CategoryId
                };

                return ServiceResult<SubCategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add subcategory: {ex.Message}");
                return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add subcategory.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid subcategory ID format: {id}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Invalid subcategory ID format.");
                }

                var subCategory = await _context.SubCategories.FindAsync(id);
                if (subCategory == null)
                {
                    _logger.LogWarning($"SubCategory with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"SubCategory with ID {id} not found.");
                }

                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"SubCategory with ID {id} deleted successfully.");
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete subcategory: {ex.Message}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete subcategory.");
            }
        }

        public async Task<ServiceResult<IEnumerable<SubCategoryDTO>>> GetAll()
        {
            try
            {
                var subCategories = await _context.SubCategories.ToListAsync();
                var response = subCategories.Select(s => new SubCategoryDTO
                {
                    Id = s.SubcategoryId,
                    Name = s.SubcategoryName,
                    CategoryId = s.CategoryId
                });

                return ServiceResult<IEnumerable<SubCategoryDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve subcategories: {ex.Message}");
                return ServiceResult<IEnumerable<SubCategoryDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve subcategories.");
            }
        }

        public async Task<ServiceResult<SubCategoryDTO>> GetByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid subcategory ID format: {id}");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "Invalid subcategory ID format.");
                }

                var subCategory = await _context.SubCategories.FindAsync(id);
                if (subCategory == null)
                {
                    _logger.LogWarning($"SubCategory with ID {id} not found.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.NotFound, $"SubCategory with ID {id} not found.");
                }

                var response = new SubCategoryDTO
                {
                    Id = subCategory.SubcategoryId,
                    Name = subCategory.SubcategoryName,
                    CategoryId = subCategory.CategoryId
                };

                return ServiceResult<SubCategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve subcategory: {ex.Message}");
                return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve subcategory.");
            }
        }

        public async Task<ServiceResult<SubCategoryDTO>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("Tried to search subcategory with empty name.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "SubCategory name cannot be empty.");
                }

                var subCategory = await _context.SubCategories.FirstOrDefaultAsync(s => s.SubcategoryName == name);
                if (subCategory == null)
                {
                    _logger.LogWarning($"SubCategory with name {name} not found.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.NotFound, $"SubCategory with name {name} not found.");
                }

                var response = new SubCategoryDTO
                {
                    Id = subCategory.SubcategoryId,
                    Name = subCategory.SubcategoryName,
                    CategoryId = subCategory.CategoryId
                };

                return ServiceResult<SubCategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve subcategory by name: {ex.Message}");
                return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve subcategory.");
            }
        }

        public async Task<ServiceResult<SubCategoryDTO>> Update(UpdateSubCategoryDTO subCategoryDTO)
        {
            try
            {
                if (subCategoryDTO.id <= 0)
                {
                    _logger.LogWarning($"Invalid subcategory ID format: {subCategoryDTO.id}");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "Invalid subcategory ID format.");
                }
                if (string.IsNullOrWhiteSpace(subCategoryDTO.Name))
                {
                    _logger.LogWarning("Tried to update subcategory with empty name.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.Validation, "SubCategory name cannot be empty.");
                }

                var subCategory = await _context.SubCategories.FindAsync(subCategoryDTO.id);
                if (subCategory == null)
                {
                    _logger.LogWarning($"SubCategory with ID {subCategoryDTO.id} not found for update.");
                    return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.NotFound, $"SubCategory with ID {subCategoryDTO.id} not found.");
                }

                subCategory.SubcategoryName = subCategoryDTO.Name;
                subCategory.CategoryId = subCategoryDTO.Categoryid;

                await _context.SaveChangesAsync();

                var response = new SubCategoryDTO
                {
                    Id = subCategory.SubcategoryId,
                    Name = subCategory.SubcategoryName,
                    CategoryId = subCategory.CategoryId
                };

                _logger.LogInformation($"SubCategory with ID {subCategoryDTO.id} updated successfully.");
                return ServiceResult<SubCategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update subcategory: {ex.Message}");
                return ServiceResult<SubCategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update subcategory.");
            }
        }
    }
}
