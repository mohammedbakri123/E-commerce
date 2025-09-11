using E_commerce_Endpoints.DTO.Favorite.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoriteController : MyControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService, ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

      //  [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddFavoriteDTO dto)
        {
            var result = await _favoriteService.Add(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateFavoriteDTO dto)
        {
            var result = await _favoriteService.Update(dto);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _favoriteService.GetByID(id);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? userId = null, [FromQuery] int? variantId = null)
        {
            var result = await _favoriteService.GetAll(userId, variantId);
            return MapServiceResult(result);
        }

        // [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _favoriteService.Delete(id);
            return MapServiceResult(result);
        }
    }
}
