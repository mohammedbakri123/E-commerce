using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Product.Request;
using E_commerce_Endpoints.DTO.Product.Response;
using E_commerce_Endpoints.DTO.User.Request;
using E_commerce_Endpoints.DTO.User.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly appDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(appDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<ProductDTO>> Add(AddProductDTO productDTO)
        {
            try
            {
                if (!Validation.TryValidate(productDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalied Input : {messages}");
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.Validation, $"you Should Pass all the required data : {messages}");

                }

                bool IsDublicated = await _context.Products.AnyAsync(p => p.ProductName == productDTO.Name);
                if (IsDublicated)
                {
                    _logger.LogWarning($"Tried To Add Product with Dublicate Name : {productDTO.Name} ");
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.Duplicate, $"Tried To Add Product with Dublicate Name :  {productDTO.Name}  ");
                }

                var product = new Product
                {
                    ProductName = productDTO.Name,
                    BrandId = productDTO.BrandId,
                    SubcategoryId = productDTO.SubCategoryId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var response = await GetByID(product.ProductId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add product: {ex.Message}, at {DateTime.Now.ToString()}");
                return ServiceResult<ProductDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add product.");
            }
        }

        public async Task<ServiceResult<ProductDTO>> Update(UpdateProductDTO productDTO)
        {
            try
            {
                if (!Validation.TryValidate(productDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalied Input : {messages}");
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.Validation, $"you Should Pass all the required data : {messages}");

                }

                var product = await _context.Products.FindAsync(productDTO.Id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID : {productDTO.Id}, was not found");
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.NotFound, $"Product with ID {productDTO.Id} not found.");
                }

               
                    product.ProductName = productDTO.Name;
                    product.BrandId = productDTO.BrandId;
                    product.SubcategoryId = productDTO.SubCategoryId;
                    product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = await GetByID(product.ProductId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update product: {ex.Message}, at {DateTime.Now.ToString()}");
                return ServiceResult<ProductDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update product.");
            }
        }

        public async Task<ServiceResult<ProductDTO>> GetByID(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Subcategory)
                        .ThenInclude(sc => sc.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.NotFound, $"Product with ID {id} not found.");

                var response = new ProductDTO
                {
                    Id = product.ProductId,
                    Name = product.ProductName,
                    BrandID = product.BrandId,
                    BrandName = product.Brand?.BrandName,
                    SubCategoryID = product.SubcategoryId,
                    SubCategoryName = product.Subcategory?.SubcategoryName,
                    CategoryID = product.Subcategory?.CategoryId,
                    CategoryName = product.Subcategory?.Category?.CategoryName,
                    CreatedDate = product.CreatedAt,
                    UpdatedDate = product.UpdatedAt,
                    
                    
                };

                return ServiceResult<ProductDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get product by ID: {ex.Message}, at {DateTime.Now.ToString()}");
                return ServiceResult<ProductDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get product.");
            }
        }

        public async Task<ServiceResult<ProductDTO>> GetByName(string name)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Subcategory)
                        .ThenInclude(sc => sc.Category)
                    .FirstOrDefaultAsync(p => p.ProductName == name);

                if (product == null)
                {
                    _logger.LogWarning($"Product was Not Found Name : {name}");
                    return ServiceResult<ProductDTO>.Fail(ServiceErrorType.NotFound, $"Product with name '{name}' not found.");
                }

                var response = new ProductDTO
                {
                    Id = product.ProductId,
                    Name = product.ProductName,
                    BrandID = product.BrandId ?? 0,
                    BrandName = product.Brand?.BrandName,
                    SubCategoryID = product.SubcategoryId ?? 0,
                    SubCategoryName = product.Subcategory?.SubcategoryName,
                    CategoryID = product.Subcategory?.CategoryId ?? 0,
                    CategoryName = product.Subcategory?.Category?.CategoryName,
                    CreatedDate = product.CreatedAt,
                    UpdatedDate = product.UpdatedAt,
                };

                return ServiceResult<ProductDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get product by name: {ex.Message}, at {DateTime.Now.ToString()}");
                return ServiceResult<ProductDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get product.");
            }
        }

        public async Task<ServiceResult<IEnumerable<ProductDTO>>> GetAll(int? brandId = null, int? categoryId = null, int? subCategoryId = null, string? search = null)
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Subcategory)
                        .ThenInclude(sc => sc.Category)
                    .AsQueryable();

                if (brandId.HasValue)
                    query = query.Where(p => p.BrandId == brandId.Value);
                if (categoryId.HasValue)
                    query = query.Where(p => p.Subcategory != null && p.Subcategory.CategoryId == categoryId.Value);
                if (subCategoryId.HasValue)
                    query = query.Where(p => p.SubcategoryId == subCategoryId.Value);
                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(p => p.ProductName.Contains(search));

                var products = await query.ToListAsync();

                var response = products.Select(p => new ProductDTO
                {
                    Id = p.ProductId,
                    Name = p.ProductName,
                    BrandID = p.BrandId ?? 0,
                    BrandName = p.Brand?.BrandName,
                    SubCategoryID = p.SubcategoryId ?? 0,
                    SubCategoryName = p.Subcategory?.SubcategoryName,
                    CategoryID = p.Subcategory?.CategoryId ?? 0,
                    CategoryName = p.Subcategory?.Category?.CategoryName,
                    CreatedDate = p.CreatedAt,
                    UpdatedDate = p.UpdatedAt
                });

                return ServiceResult<IEnumerable<ProductDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get all products: {ex.Message}, at {DateTime.Now.ToString()}");
                return ServiceResult<IEnumerable<ProductDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get products.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
               
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Product with ID {id} not found.");

                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete product: {ex.Message} , at {DateTime.Now.ToString()}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete product.");
            }
        }
    }
}
