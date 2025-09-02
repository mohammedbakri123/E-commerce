using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.Authentication.Request;
using E_commerce_Endpoints.DTO.Authentication.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDto);
        Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO loginDto);
        string GenerateJwtToken(User user);
    }
}
