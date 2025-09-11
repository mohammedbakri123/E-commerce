using E_commerce_Endpoints.DTO.Stock.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : MyControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockService stockService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddStockDTO dto)
        {
            var result = await _stockService.Add(dto);
            return MapServiceResult(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateStockDTO dto)
        {
            var result = await _stockService.Update(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _stockService.GetByID(id);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? variantId = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] bool? isDone = null)
        {
            var result = await _stockService.GetAll(variantId, supplierId, isDone);
            return MapServiceResult(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _stockService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
