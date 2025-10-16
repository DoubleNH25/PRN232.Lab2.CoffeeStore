using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using System.Text.Json;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOrStaff")] // Require authentication for all endpoints
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get all orders with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseModel>>> GetOrders(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null,
            [FromQuery] int? userId = null)
        {
            var (orders, totalCount) = await _orderService.GetOrdersAsync(search, sortBy, ascending, page, pageSize, fields, userId);

            // Add pagination headers
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            }));

            // Apply field selection if specified
            if (!string.IsNullOrEmpty(fields))
            {
                var selectedOrders = SelectFields(orders, fields);
                return Ok(selectedOrders);
            }

            return Ok(orders);
        }

        /// <summary>
        /// Get a specific order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseModel>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create orders
        public async Task<ActionResult<OrderResponseModel>> CreateOrder(OrderRequestModel orderRequest)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(orderRequest);
                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing order
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can update orders
        public async Task<ActionResult<OrderResponseModel>> UpdateOrder(int id, OrderRequestModel orderRequest)
        {
            try
            {
                var order = await _orderService.UpdateOrderAsync(id, orderRequest);
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an order
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete orders
        public async Task<ActionResult<bool>> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private IEnumerable<object> SelectFields(IEnumerable<OrderResponseModel> orders, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var order in orders)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("orderid") || fieldList.Contains("id"))
                    obj["orderId"] = order.OrderId;

                if (fieldList.Contains("userid"))
                    obj["userId"] = order.UserId;

                if (fieldList.Contains("orderdate"))
                    obj["orderDate"] = order.OrderDate;

                if (fieldList.Contains("status"))
                    obj["status"] = order.Status;

                if (fieldList.Contains("totalamount"))
                    obj["totalAmount"] = order.TotalAmount;

                if (fieldList.Contains("username"))
                    obj["userName"] = order.UserName;

                yield return obj;
            }
        }
    }
}