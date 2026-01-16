using Microsoft.AspNetCore.Mvc;
using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Interfaces;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetOrders()
        {
            return Ok(await _orderService.GetOrders());
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateOrder(request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto request)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid OrderId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateOrder(id, request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid OrderId");
            }

            var result = await _orderService.DeleteOrder(id);

            return (result) ? NoContent() : NotFound();
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid OrderId");
            }

            var result = await _orderService.GetOrder(id);

            if (result == null)
            {
                return NotFound("The order don't exists");
            }

            return Ok(result);
        }

        [HttpPost("changestatus/{id}")]
        public async Task<ActionResult> ChangeStatus([FromBody]OrderStatus status, int id)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                return BadRequest(new { error = "Invalid status value" });
            }

            var result = await _orderService.ChangeStatus(status, id);

            return (result) ? NoContent() : NotFound();
        }
    }
}
