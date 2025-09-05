using E_commerce_Endpoints.DTO.Offer.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferController : MyControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly ILogger<OfferController> _logger;

        public OfferController(IOfferService offerService, ILogger<OfferController> logger)
        {
            _offerService = offerService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddOfferDTO dto)
        {
            var result = await _offerService.Add(dto);
            return MapServiceResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOfferDTO dto)
        {
            var result = await _offerService.Update(dto);
            return MapServiceResult(result);
        }

        [Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _offerService.GetByID(id);
            return MapServiceResult(result);
        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? variantId = null)
        {
            var result = await _offerService.GetAll(variantId);
            return MapServiceResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _offerService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
