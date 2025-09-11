using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.User.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : MyControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //  [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO dto)
        {
            var result = await _userService.AddUserAsync(dto);
            return MapServiceResult(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string? role, [FromQuery] bool? activeOnly)
        {
            var result = await _userService.GetAllAsync(role, activeOnly);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("byEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDTO dto)
        {
            var result = await _userService.UpdateAsync(dto);
            return MapServiceResult(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteByIDAsync(id);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var result = await _userService.ChangePassword(dto);
            return MapServiceResult(result);
        }
    }
}
