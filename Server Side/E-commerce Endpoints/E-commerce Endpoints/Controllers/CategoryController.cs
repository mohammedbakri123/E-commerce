using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : MyControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        //[Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByID(id);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _categoryService.GetByName(name);
            return MapServiceResult(result);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddCategoryDTO dto)
        {
            var result = await _categoryService.Add(dto);
            return MapServiceResult(result);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDTO dto)
        {
            var result = await _categoryService.Update(dto);
            return MapServiceResult(result);
        }

       // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
