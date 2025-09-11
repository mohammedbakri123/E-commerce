using E_commerce_Endpoints.DTO.Supplier.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController : MyControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddSupplierDTO dto)
        {
            var result = await _supplierService.Add(dto);
            return MapServiceResult(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSupplierDTO dto)
        {
            var result = await _supplierService.Update(dto);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _supplierService.GetByID(id);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supplierService.GetAll();
            return MapServiceResult(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _supplierService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
