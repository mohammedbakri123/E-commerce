using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Authentication.Request;
using E_commerce_Endpoints.DTO.Authentication.Response;
using E_commerce_Endpoints.DTO.User.Request;
using E_commerce_Endpoints.DTO.User.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly appDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(IConfiguration configuration , appDbContext context , ILogger<UserService> logger) 
        { 
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<UserDTO>> AddUserAsync(AddUserDTO addUserDTO)
        {
            try
            {
                if (!Validation.TryValidate(addUserDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalied Input : {messages}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, $"you Should Pass all the required data : {messages}");

                }

                if (!Validation.IsValidEmail(addUserDTO.Email))
                {
                    _logger.LogWarning("tried to Add Invalied Email");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, "Email is not Valid");
                }

                if (await _context.Users.AnyAsync(u => u.Email == addUserDTO.Email))
                {
                    _logger.LogWarning("Dublicate Email registeration");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Duplicate, "Email is already Exist");
                }
                if (!(addUserDTO.Password == addUserDTO.ConfirmPassword))
                {
                    _logger.LogWarning("Password And Confiramtion Is not the same");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, "Password And Confirmation Should be Equal");
                }

                if (!Validation.IsStrongPassword(addUserDTO.Password))
                {
                    _logger.LogWarning("Password Entered is so weak");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, "Password Should containt Minimum 8 chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special char");
                }
                var passwordHash = PasswordHelper.HashPassword(addUserDTO.Password);
                var user = new User
                {
                    Email = addUserDTO.Email,
                    UserFirstName = addUserDTO.FirstName != "" ? addUserDTO.FirstName : UserHelper.GetUserName(addUserDTO.Email),
                    Password = passwordHash,
                    Role = addUserDTO.Role,
                    Status = true,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                UserDTO response = new UserDTO
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status,
                    Name = user.UserFirstName,
                };
                return ServiceResult<UserDTO>.Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Server Failed to add User : " + ex.Message + " at : " + DateTime.Now.ToString());
                return ServiceResult<UserDTO>.Fail(ServiceErrorType.ServerError, "Server Failed to add User " + ex.Message + " at : " + DateTime.Now.ToString());
            }
        }

        public async Task<ServiceResult<bool>> ChangePassword(ChangePasswordDTO changePassword)
        {
            try
            {
                if (!Validation.TryValidate(changePassword, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalied Input : {messages}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, $"you Should Pass all the required data : {messages}");
                }
                if (!Validation.IsValidEmail(changePassword.Email))
                {
                    _logger.LogWarning("tried to Add Invalied Email");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Email is not Valid");
                }
                User currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == changePassword.Email);
                if (currentUser == null)
                {
                    _logger.LogWarning("Email not Found");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, "Email you entered dose not exist");
                }

                if (!PasswordHelper.VerifyPassword(changePassword.CurrentPassword , currentUser.Password))
                {
                    _logger.LogWarning("tried to change password, but the old password entered is not right");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "old Password is not right");
                }
                if (!(changePassword.NewPassword == changePassword.ConfirmNewPassword))
                {
                    _logger.LogWarning("Password And Confiramtion Is not the same");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Password And Confirmation Should be Equal");
                }

                if (!Validation.IsStrongPassword(changePassword.NewPassword))
                {
                    _logger.LogWarning("Password Entered is so weak");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, "Password Should containt Minimum 8 chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special char");
                }

                currentUser.Password = PasswordHelper.HashPassword(changePassword.NewPassword);
                _context.Users.Update(currentUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Password changed successfully for user: {currentUser.Email}");
                return ServiceResult<bool>.Ok(true);


            }
            catch(Exception ex)
            {
                _logger.LogError("Server Failed to Change Password : " + ex.Message + " at : " + DateTime.Now.ToString());
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, "Server Failed to Change Password " + ex.Message + " at : " + DateTime.Now.ToString());
            }
        }

        public async Task<ServiceResult<bool>> DeleteByIDAsync(int id)
        {
            try
            {
                
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid user ID: {id}");
                    return ServiceResult<bool>.Fail(ServiceErrorType.Validation, $"User ID '{id}' is not valid.");
                }
                var currentUser = await _context.Users.FindAsync(id);
                if (currentUser == null)
                {
                    _logger.LogWarning($"User with ID {id} not found");
                    return ServiceResult<bool>.Fail(ServiceErrorType.NotFound, $"User with ID {id} not found");
                }

                _context.Users.Remove(currentUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User with Email {currentUser.Email} deleted successfully");
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to delete user: {ex.Message} at {DateTime.Now}");
                return ServiceResult<bool>.Fail(ServiceErrorType.ServerError, $"Server failed to delete user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<UserDTO>>> GetAllAsync(string? role = null, bool? activeOnly = null)
        {
            try
            {
                
                var query = _context.Users.AsQueryable();

          
                if (!string.IsNullOrEmpty(role))
                {
                    query = query.Where(u => u.Role == role);
                }

                if (activeOnly.HasValue && activeOnly.Value)
                {
                    query = query.Where(u => u.Status == true);
                }

              
                var users = await query
                    .Select(u => new UserDTO
                    {
                        Id = u.UserId,
                        Name = u.UserFirstName,
                        Email = u.Email,
                        Role = u.Role,
                        Status = u.Status ?? false,
                        
                    })
                    .ToListAsync();

                if(!(users.Count > 0))
                {
                    _logger.LogWarning($"No user Found");
                    return ServiceResult<IEnumerable<UserDTO>>.Fail(
                        ServiceErrorType.NotFound,
                        $"No user Found"
                    );
                }

                return ServiceResult<IEnumerable<UserDTO>>.Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to retrieve users: {ex.Message} at {DateTime.Now}");
                return ServiceResult<IEnumerable<UserDTO>>.Fail(
                    ServiceErrorType.ServerError,
                    $"Server failed to retrieve users: {ex.Message}"
                );
            }
        }


        public async Task<ServiceResult<UserDTO>> GetByEmailAsync(string email)
        {
            try
            {
                if (!Validation.IsValidEmail(email))
                {
                    _logger.LogWarning($"Tried to enter invalid email: {email}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, $"Email '{email}' is not valid.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning($"Could not find user with email: {email}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.NotFound, $"User with email '{email}' not found.");
                }

                var response = new UserDTO
                {
                    Id = user.UserId,
                    Name = user.UserFirstName,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status ?? false,
                    
                };

                return ServiceResult<UserDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get user with email: {email}, {ex.Message} at {DateTime.Now}");
                return ServiceResult<UserDTO>.Fail(
                    ServiceErrorType.ServerError,
                    $"Server failed to get user: {ex.Message}"
                );
            }

        }

        public async Task<ServiceResult<UserDTO>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid user ID: {id}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, $"User ID '{id}' is not valid.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.NotFound, $"User with ID '{id}' not found.");
                }

                var response = new UserDTO
                {
                    Id = user.UserId,
                    Name = user.UserFirstName,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status ?? false,
                  
                };

                return ServiceResult<UserDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to get user with ID {id}: {ex.Message} at {DateTime.Now}");
                return ServiceResult<UserDTO>.Fail(
                    ServiceErrorType.ServerError,
                    $"Server failed to get user: {ex.Message}"
                );
            }
        }

        public async Task<ServiceResult<UserDTO>> UpdateAsync(UpdateUserDTO updateUserDTO)
        {
            try
            {
                // Validate input
                if (!Validation.TryValidate(updateUserDTO, out var validationErrors))
                {
                    var messages = string.Join("; ", validationErrors.Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid input: {messages}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, $"You should pass all the required data: {messages}");
                }

                if (!Validation.IsValidEmail(updateUserDTO.Email))
                {
                    _logger.LogWarning($"Invalid email: {updateUserDTO.Email}");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Validation, "Email is not valid.");
                }
                if (await _context.Users.AnyAsync(u => u.Email == updateUserDTO.Email))
                {
                    _logger.LogWarning("Dublicate Email registeration");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Duplicate, "Email is already Exist");
                }
                if (await _context.Users.AnyAsync(u => u.Email == updateUserDTO.Email))
                {
                    _logger.LogWarning("Dublicate Email registeration");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.Duplicate, "Email is already Exist");
                }

                // Find existing user
                var user = await _context.Users.FindAsync(updateUserDTO.UserId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {updateUserDTO.UserId} not found");
                    return ServiceResult<UserDTO>.Fail(ServiceErrorType.NotFound, $"User with ID {updateUserDTO.UserId} not found.");
                }
       

                // Check for email duplication if the email is changing
                if (user.Email != updateUserDTO.Email)
                {
                    bool emailExists = await _context.Users.AnyAsync(u => u.Email == updateUserDTO.Email);
                    if (emailExists)
                    {
                        _logger.LogWarning($"Email already exists: {updateUserDTO.Email}");
                        return ServiceResult<UserDTO>.Fail(ServiceErrorType.Duplicate, "Email already exists.");
                    }
                }

                // Update user properties
                user.UserFirstName = updateUserDTO.FirstName;
                user.Email = updateUserDTO.Email;
                user.Role = updateUserDTO.Role;
                user.Status = updateUserDTO.Status;

                await _context.SaveChangesAsync();

                // Map to DTO
                var response = new UserDTO
                {
                    Id = user.UserId,
                    Name = user.UserFirstName,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status,
                    
                };

                _logger.LogInformation($"User with ID {user.UserId} updated successfully.");
                return ServiceResult<UserDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server failed to update user with ID {updateUserDTO?.UserId}: {ex.Message} at {DateTime.Now}");
                return ServiceResult<UserDTO>.Fail(ServiceErrorType.ServerError, $"Server failed to update user: {ex.Message}");
            }
        }
    }
}
