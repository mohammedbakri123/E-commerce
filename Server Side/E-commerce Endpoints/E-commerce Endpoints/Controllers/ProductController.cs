using E_commerce_Endpoints.DTO.Product.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : MyControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductDTO dto)
        {
            var result = await _productService.Add(dto);
            return MapServiceResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto)
        {
            var result = await _productService.Update(dto);
            return MapServiceResult(result);
        }

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _productService.GetByID(id);
            return MapServiceResult(result);
        }

        [HttpGet("getByName")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _productService.GetByName(name);
            return MapServiceResult(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? brandId = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? subCategoryId = null,
            [FromQuery] string? search = null)
        {
            var result = await _productService.GetAll(brandId, categoryId, subCategoryId, search);
            return MapServiceResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _productService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
