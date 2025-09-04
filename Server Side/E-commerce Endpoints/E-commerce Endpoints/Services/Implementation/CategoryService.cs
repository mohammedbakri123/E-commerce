using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.DTO.Category.Response;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;
using E_commerce_Endpoints.Helper;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly appDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(appDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<CategoryDTO>> Add(AddCategoryDTO categoryDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryDTO.Name))
                {
                    _logger.LogWarning("Tried to add a category with empty name.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Validation, "Category name cannot be empty.");
                }

                if (await _context.Categories.AnyAsync(c => c.CategoryName == categoryDTO.Name))
                {
                    _logger.LogWarning($"Duplicate category creation attempt: {categoryDTO.Name}");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Duplicate, "Category already exists.");
                }

                var category = new Category
                {
                    CategoryName = categoryDTO.Name,
                    
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                
                var response = new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                   
                };

                return ServiceResult<CategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add category: {ex.Message}, at : {DateTime.Now.ToString()}");
                return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add category.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid category ID format: {id}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Invalid category ID format.");
                }

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Category with ID {id} not found.");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Category with ID {id} deleted successfully.");
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete category: {ex.Message} at : {DateTime.Now.ToString()}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete category.");
            }
        }

        public async Task<ServiceResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                var response = categories.Select(c => new CategoryDTO
                {
                    Id = c.CategoryId,
                    Name = c.CategoryName,
                    
                });

                return ServiceResult<IEnumerable<CategoryDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve categories: {ex.Message} at : {DateTime.Now.ToString()}");
                return ServiceResult<IEnumerable<CategoryDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve categories.");
            }
        }

        public async Task<ServiceResult<CategoryDTO>> GetByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid category ID format: {id}");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Validation, "Invalid category ID format.");
                }

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.NotFound, $"Category with ID {id} not found.");
                }

                var response = new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                   
                };

                return ServiceResult<CategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve category by ID: {ex.Message}, at : {DateTime.Now.ToString()}");
                return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve category.");
            }
        }

        public async Task<ServiceResult<CategoryDTO>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("Tried to search category with empty name.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Validation, "Category name cannot be empty.");
                }

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
                if (category == null)
                {
                    _logger.LogWarning($"Category with name {name} not found.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.NotFound, $"Category with name {name} not found.");
                }

                var response = new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                };

                return ServiceResult<CategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve category by name: {ex.Message} at : {DateTime.Now.ToString()}");
                return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to retrieve category.");
            }
        }

        public async Task<ServiceResult<CategoryDTO>> Update(UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO.id <= 0)
                {
                    _logger.LogWarning($"Invalid category ID format: {categoryDTO.id}");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Validation, "Invalid category ID format.");
                }
                if (string.IsNullOrWhiteSpace(categoryDTO.Name))
                {
                    _logger.LogWarning("Tried to search category with empty name.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.Validation, "Category name cannot be empty.");
                }

                var category = await _context.Categories.FindAsync(categoryDTO.id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {categoryDTO.id} not found for update.");
                    return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.NotFound, $"Category with ID {categoryDTO.id} not found.");
                }

                
                    category.CategoryName = categoryDTO.Name;


                await _context.SaveChangesAsync();

                var response = new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                };

                _logger.LogInformation($"Category with ID {categoryDTO.id} updated successfully.");
                return ServiceResult<CategoryDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update category: {ex.Message} at : {DateTime.Now.ToString()}");
                return ServiceResult<CategoryDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update category.");
            }
        }
    }
}
