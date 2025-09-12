using E_commerce_Endpoints.DTO.CartItem.Request;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartItemController : MyControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartItemController> _logger;

        public CartItemController(ICartItemService cartItemService, ILogger<CartItemController> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddCartItemDTO dto)
        {
            var result = await _cartItemService.AddItem(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCartItemDTO dto)
        {
            var result = await _cartItemService.UpdateItem(dto);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpDelete("delete/{cartItemId:int}")]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            var result = await _cartItemService.DeleteItem(cartItemId);
            return MapServiceResult(result);
        }

        //[Authorize]
        [HttpGet("getAll/{cartId:int}")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            var result = await _cartItemService.GetCartItems(cartId);
            return MapServiceResult(result);
        }
    }
}
