using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Admin.Request;
using E_commerce_Endpoints.DTO.Admin.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly appDbContext _context;
        private readonly ILogger<AdminService> _logger;

        public AdminService(appDbContext context, ILogger<AdminService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<AdminDTO>> Add(AddAdminDTO dto)
        {
            try
            {
                
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input while adding admin: {messages}");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                {
                    _logger.LogWarning($"Attempted to add admin but user with ID {dto.UserId} does not exist.");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.NotFound, $"User with ID {dto.UserId} not found.");
                }

               
                bool isAlreadyAdmin = await _context.Admins.AnyAsync(a => a.UserId == dto.UserId);
                if (isAlreadyAdmin)
                {
                    _logger.LogWarning($"Attempted to add admin but user with ID {dto.UserId} is already an admin.");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.Duplicate, $"User with ID {dto.UserId} is already an admin.");
                }

                
                var admin = new Admin
                {
                    UserId = dto.UserId,
                    AdminLastName = dto.AdminLastName,
                    PermissionsValue = dto.PermissionsValue,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address, 
                };

                

                _context.Admins.Add(admin);
                user.Role = "Admin";
                await _context.SaveChangesAsync();

                return await GetByID(admin.AdminId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to add admin: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<AdminDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to add admin. {ex.Message}");
            }
        }

        public async Task<ServiceResult<AdminDTO>> Update(UpdateAdminDTO dto)
        {
            try
            {
                if (!Validation.TryValidate(dto, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input: {messages}");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.Validation, $"Invalid data: {messages}");
                }

                var admin = await _context.Admins.FindAsync(dto.AdminId);
                if (admin == null)
                {
                    _logger.LogWarning($"Admin with ID {dto.AdminId} not found.");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.NotFound, $"Admin with ID {dto.AdminId} not found.");
                }
                admin.AdminLastName = dto.AdminLastName;
                admin.PermissionsValue = dto.PermissionsValue;
                admin.PhoneNumber = dto.PhoneNumber;
                admin.Address = dto.Address;

                await _context.SaveChangesAsync();

                return await GetByID(admin.AdminId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update admin: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<AdminDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to update admin. {ex.Message}");
            }
        }

        public async Task<ServiceResult<AdminDTO>> GetByID(int id)
        {
            try
            {
                var admin = await _context.Admins
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AdminId == id);

                if (admin == null)
                {
                    _logger.LogWarning($"Admin with ID {id} not found.");
                    return ServiceResult<AdminDTO>.Fail(ServiceErrorType.NotFound, $"Admin with ID {id} not found.");
                }
                var response = new AdminDTO
                {
                    AdminId = admin.AdminId,
                    UserId = admin.UserId,
                    AdminLastName = admin.AdminLastName,
                    PermissionsValue = admin.PermissionsValue,
                    PhoneNumber = admin.PhoneNumber,
                    Address = admin.Address,
                    UserName = admin.User.UserFirstName
                };

                return ServiceResult<AdminDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get admin by ID: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<AdminDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to get admin. {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<AdminDTO>>> GetAll()
        {
            try
            {
                var admins = await _context.Admins.Include(a => a.User).ToListAsync();

                var response = admins.Select(admin => new AdminDTO
                {
                    AdminId = admin.AdminId,
                    UserId = admin.UserId,
                    AdminLastName = admin.AdminLastName,
                    PermissionsValue = admin.PermissionsValue,
                    PhoneNumber = admin.PhoneNumber,
                    Address = admin.Address,
                    UserName = admin.User.UserFirstName
                });

                return ServiceResult<IEnumerable<AdminDTO>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get all admins: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<IEnumerable<AdminDTO>>.Fail(ServiceErrorType.ServerError, $"Server failed to get admins. {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var admin = await _context.Admins.FindAsync(id);
                if (admin == null)
                {
                    _logger.LogWarning($"Admin with ID {id} not found.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Admin with ID {id} not found.");
                }

                var user = await _context.Users.FindAsync(admin.UserId);
                if(user == null)
                {
                    _logger.LogWarning($"Admin with ID {id} is found, but no user conected with it, fails to find user with id : {admin.UserId}.");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"Admin with ID {id} is found, but no user conected with it, fails to find user with id : {admin.UserId}.");
                }

                user.Role = "Customer";
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete admin: {ex.Message}, at {DateTime.UtcNow}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, $"Server failed to delete admin. {ex.Message}");
            }
        }
    }
}

