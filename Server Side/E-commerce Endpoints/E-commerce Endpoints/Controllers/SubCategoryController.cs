using E_commerce_Endpoints.DTO.Category.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubCategoryController : MyControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ILogger<SubCategoryController> _logger;

        public SubCategoryController(ISubCategoryService subCategoryService, ILogger<SubCategoryController> logger)
        {
            _subCategoryService = subCategoryService;
            _logger = logger;
        }

        //[Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _subCategoryService.GetAll();
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _subCategoryService.GetByID(id);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _subCategoryService.GetByName(name);
            return MapServiceResult(result);
        }

        //  [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddSubCategoryDTO dto)
        {
            var result = await _subCategoryService.Add(dto);
            return MapServiceResult(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSubCategoryDTO dto)
        {
            var result = await _subCategoryService.Update(dto);
            return MapServiceResult(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subCategoryService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
