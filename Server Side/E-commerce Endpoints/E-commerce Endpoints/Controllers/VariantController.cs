using E_commerce_Endpoints.DTO.Variant.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VariantController : MyControllerBase
    {
        private readonly IVariantService _variantService;
        private readonly ILogger<VariantController> _logger;

        public VariantController(IVariantService variantService, ILogger<VariantController> logger)
        {
            _variantService = variantService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddVariantDTO dto)
        {
            var result = await _variantService.Add(dto);
            return MapServiceResult(result);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateVariantDTO dto)
        {
            var result = await _variantService.Update(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _variantService.GetByID(id);
            return MapServiceResult(result);
        }

       // [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? productId = null,
            [FromQuery] bool? status = null)
        {
            var result = await _variantService.GetAll(productId, status);
            return MapServiceResult(result);
        }

      //  [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _variantService.Delete(id);
            return MapServiceResult(result);
        }
        //[Authorize]
        [HttpGet("getWithPriceAndQuantity/{id:int}")]
        public async Task<IActionResult> GetWithPriceAndQuantity (int id)
        {
            var result = await _variantService.GetByIDWithPriceAndQuantity(id);
            return MapServiceResult(result);

        }
       // [Authorize]
        [HttpGet("GetAllWithPriceAndQuantity")]
        public async Task<IActionResult> GetAllWithPriceAndQuantity(
         [FromQuery] int? productId = null,
         [FromQuery] bool? status = null)
        {
            var result = await _variantService.GetAllWithQuantityAndPrice(productId, status);
            return MapServiceResult(result);
        }
    }
}
