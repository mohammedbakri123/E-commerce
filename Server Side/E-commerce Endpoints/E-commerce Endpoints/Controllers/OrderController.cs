using E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.DTO.Order.Request.E_commerce_Endpoints.DTO.Order.Request;
using E_commerce_Endpoints.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : MyControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddOrderDTO dto)
        {
            var result = await _orderService.Add(dto);
            return MapServiceResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDTO dto)
        {
            var result = await _orderService.Update(dto);
            return MapServiceResult(result);
        }

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderService.GetById(id);
            return MapServiceResult(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            return MapServiceResult(result);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id);
            return MapServiceResult(result);
        }

        [HttpPut("ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusDTO dto)
        {
            var result = await _orderService.ChangeStatus(dto);
            return MapServiceResult(result);
        }
    }
}
