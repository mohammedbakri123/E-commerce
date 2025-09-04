using E_commerce_Endpoints.DTO.Authentication.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : MyControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return MapServiceResult(result);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return MapServiceResult(result);
        }
    }
}
