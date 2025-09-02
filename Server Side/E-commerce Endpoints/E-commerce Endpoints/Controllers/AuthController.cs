using E_commerce_Endpoints.DTO.Authentication.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
            {
                return result.Error.Type switch
                {
                    ServiceErrorType.Duplicate => Conflict(result.Error),
                    ServiceErrorType.Validation => BadRequest(result.Error),
                    ServiceErrorType.Unauthorized => Unauthorized(result.Error),
                    ServiceErrorType.NotFound => NotFound(result.Error),
                    ServiceErrorType.ServerError => StatusCode(500, result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return result.Error.Type switch
                {
                    ServiceErrorType.Unauthorized => Unauthorized(result.Error),
                    ServiceErrorType.ServerError => StatusCode(500, result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok(result.Data);
        }
    }
}
