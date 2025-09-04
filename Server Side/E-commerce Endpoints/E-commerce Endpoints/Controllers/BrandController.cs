using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : MyControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandController> _logger;

        public BrandController(IBrandService brandService, ILogger<BrandController> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAll();
            return MapServiceResult(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _brandService.GetByID(id);
            return MapServiceResult(result);
        }

        [Authorize]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _brandService.GetByName(name);
            return MapServiceResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddBrandDTO dto)
        {
            var result = await _brandService.Add(dto);
            return MapServiceResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateBrandDTO dto)
        {
            var result = await _brandService.Update(dto);
            return MapServiceResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
