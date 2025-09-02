using Azure;
using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Authentication.Request;
using E_commerce_Endpoints.DTO.Authentication.Response;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_commerce_Endpoints.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly appDbContext _context;

        public AuthService(appDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public string GenerateJwtToken(User user)
        {
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role) // role comes from DB
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            if (!Validation.IsValidEmail(loginDto.Email))
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.Validation, "Email is not Valid");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.NotFound, "User with This Email Not Found");

            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.Password))
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.Unauthorized, "Invalid credentials");
         

            var token = GenerateJwtToken(user);


            var response = new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                UserId = user.UserId
            };

            return ServiceResult<AuthResponseDTO>.Ok(response);

        }

        public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.Duplicate, "Email is already Exist");
            //throw new Exception("User already exists");
            if(!Validation.IsValidEmail(registerDto.Email))
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.Validation, "Email is not Valid");

            if (!(registerDto.Password == registerDto.ConfirmPassword))
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.Validation, "Password And Confirmation Should be Equal");

            try {
                var passwordHash = PasswordHelper.HashPassword(registerDto.Password);

                var user = new User
                {
                    Email = registerDto.Email,
                    UserFirstName = registerDto.Name != "" ? registerDto.Name : UserHelper.GetUserName(registerDto.Email),
                    Password = passwordHash,
                    Role = "Customer",
                    Status = true,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // return token
                var token = GenerateJwtToken(user);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    Role = user.Role,
                    UserId = user.UserId
                };

                return ServiceResult<AuthResponseDTO>.Ok(response);
            }
            catch
            {
                return ServiceResult<AuthResponseDTO>.Fail(ServiceErrorType.ServerError, "Server Failed to add User");
            }
   
        }
    }
}
