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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Get all order details with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailResponseModel>>> GetOrderDetails(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null,
            [FromQuery] int? orderId = null)
        {
            var (orderDetails, totalCount) = await _orderDetailService.GetOrderDetailsAsync(search, sortBy, ascending, page, pageSize, fields, orderId);

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
                var selectedOrderDetails = SelectFields(orderDetails, fields);
                return Ok(selectedOrderDetails);
            }

            return Ok(orderDetails);
        }

        /// <summary>
        /// Get a specific order detail by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailResponseModel>> GetOrderDetail(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        /// <summary>
        /// Create a new order detail
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create order details
        public async Task<ActionResult<OrderDetailResponseModel>> CreateOrderDetail(OrderDetailRequestModel orderDetailRequest)
        {
            try
            {
                var orderDetail = await _orderDetailService.CreateOrderDetailAsync(orderDetailRequest);
                return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing order detail
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can update order details
        public async Task<ActionResult<OrderDetailResponseModel>> UpdateOrderDetail(int id, OrderDetailRequestModel orderDetailRequest)
        {
            try
            {
                var orderDetail = await _orderDetailService.UpdateOrderDetailAsync(id, orderDetailRequest);
                if (orderDetail == null)
                {
                    return NotFound();
                }

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an order detail
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete order details
        public async Task<ActionResult<bool>> DeleteOrderDetail(int id)
        {
            var result = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private IEnumerable<object> SelectFields(IEnumerable<OrderDetailResponseModel> orderDetails, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var orderDetail in orderDetails)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("orderdetailid") || fieldList.Contains("id"))
                    obj["orderDetailId"] = orderDetail.OrderDetailId;

                if (fieldList.Contains("orderid"))
                    obj["orderId"] = orderDetail.OrderId;

                if (fieldList.Contains("productid"))
                    obj["productId"] = orderDetail.ProductId;

                if (fieldList.Contains("quantity"))
                    obj["quantity"] = orderDetail.Quantity;

                if (fieldList.Contains("unitprice"))
                    obj["unitPrice"] = orderDetail.UnitPrice;

                if (fieldList.Contains("productname"))
                    obj["productName"] = orderDetail.ProductName;

                yield return obj;
            }
        }
    }
}