using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Service.Common;

namespace TableTennis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            Console.WriteLine($"Primljeni userId: {order.UserId}");

            if (order.UserId == Guid.Empty)
            {
                return BadRequest("Neispravan ID korisnika.");
            }

            try
            {
                await _orderService.AddOrderAsync(order);
                return Ok("Narudžba je uspješno kreirana.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
