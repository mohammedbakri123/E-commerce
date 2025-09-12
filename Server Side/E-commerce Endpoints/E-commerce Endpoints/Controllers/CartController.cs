using E_commerce_Endpoints.DTO.Cart.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : MyControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AddCartIDTO dto)
        {
            var result = await _cartService.CreateCart(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? userId = null)
        {
            var result = await _cartService.GetAll(userId);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _cartService.GetById(id);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("getLast/{userId:int}")]
        public async Task<IActionResult> GetLastCart(int userId)
        {
            var result = await _cartService.GetLastCart(userId);
            return MapServiceResult(result);
        }
    }
}
