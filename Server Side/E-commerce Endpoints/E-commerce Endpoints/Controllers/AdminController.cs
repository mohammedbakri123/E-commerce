using E_commerce_Endpoints.DTO.Admin.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : MyControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddAdminDTO dto)
        {
            var result = await _adminService.Add(dto);
            return MapServiceResult(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateAdminDTO dto)
        {
            var result = await _adminService.Update(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _adminService.GetByID(id);
            return MapServiceResult(result);
        }

       // [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminService.GetAll();
            return MapServiceResult(result);
        }

       // [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
