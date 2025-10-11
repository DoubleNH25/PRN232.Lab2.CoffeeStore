using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseModel<IDictionary<string, object?>>>> GetOrders([FromQuery] PagedRequestModel request)
    {
        var result = await _orderService.GetOrdersAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IDictionary<string, object?>>> GetOrderById(int id, [FromQuery] string? select)
    {
        var result = await _orderService.GetOrderByIdAsync(id, select);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseModel>> CreateOrder([FromBody] OrderCreateRequestModel request)
    {
        var created = await _orderService.CreateOrderAsync(request);
        return CreatedAtAction(nameof(GetOrderById), new { id = created.OrderId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OrderResponseModel>> UpdateOrder(int id, [FromBody] OrderUpdateRequestModel request)
    {
        var updated = await _orderService.UpdateOrderAsync(id, request);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
