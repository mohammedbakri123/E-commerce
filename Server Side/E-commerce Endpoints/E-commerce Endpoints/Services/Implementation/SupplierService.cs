using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Supplier.Request;
using E_commerce_Endpoints.DTO.Supplier.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly appDbContext _context;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(appDbContext context, ILogger<SupplierService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<SupplierDTO>> Add(AddSupplierDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid supplier input: {messages}");
                    return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                bool exists = await _context.Suppliers.AnyAsync(s => s.CompanyName == dto.CompanyName);
                if (exists)
                {
                    _logger.LogWarning($"Supplier already exists: {dto.CompanyName}");
                    return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.Duplicate, $"Supplier {dto.CompanyName} already exists.");
                }

                var supplier = new Supplier
                {
                    CompanyName = dto.CompanyName,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address
                };

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                return await GetByID(supplier.SupplierId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add supplier: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.ServerError, "Server failed to add supplier.");
            }
        }

        public async Task<ServiceResult<SupplierDTO>> Update(UpdateSupplierDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid supplier input: {messages}");
                    return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning($"Supplier {dto.SupplierId} not found.");
                    return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.NotFound, $"Supplier {dto.SupplierId} not found.");
                }

                supplier.CompanyName = dto.CompanyName;
                supplier.PhoneNumber = dto.PhoneNumber;
                supplier.Address = dto.Address;

                await _context.SaveChangesAsync();
                return await GetByID(supplier.SupplierId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update supplier: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.ServerError, "Server failed to update supplier.");
            }
        }

        public async Task<ServiceResult<SupplierDTO>> GetByID(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning($"Supplier {id} not found.");
                    return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.NotFound, $"Supplier {id} not found.");
                }
                var response = new SupplierDTO
                {
                    SupplierId = supplier.SupplierId,
                    CompanyName = supplier.CompanyName,
                    PhoneNumber = supplier.PhoneNumber,
                    Address = supplier.Address
                };

                return ServiceResult<SupplierDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get supplier: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<SupplierDTO>.Fail(ServiceErrorType.ServerError, "Server failed to get supplier.");
            }
        }

        public async Task<ServiceResult<IEnumerable<SupplierDTO>>> GetAll()
        {
            try
            {
                var suppliers = await _context.Suppliers.ToListAsync();
                var response = suppliers.Select(s => new SupplierDTO
                {
                    SupplierId = s.SupplierId,
                    CompanyName = s.CompanyName,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address
                });

                return ServiceResult<IEnumerable<SupplierDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get all suppliers: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<IEnumerable<SupplierDTO>>.Fail(ServiceErrorType.ServerError, "Server failed to get suppliers.");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning($"Supplier {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Supplier {id} not found.");
                }

                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete supplier: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server failed to delete supplier.");
            }
        }
    }
}
