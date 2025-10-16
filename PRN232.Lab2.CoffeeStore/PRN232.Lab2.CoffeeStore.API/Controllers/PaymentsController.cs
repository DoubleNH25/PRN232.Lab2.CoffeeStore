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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get all payments with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponseModel>>> GetPayments(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null,
            [FromQuery] int? orderId = null)
        {
            var (payments, totalCount) = await _paymentService.GetPaymentsAsync(search, sortBy, ascending, page, pageSize, fields, orderId);

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
                var selectedPayments = SelectFields(payments, fields);
                return Ok(selectedPayments);
            }

            return Ok(payments);
        }

        /// <summary>
        /// Get a specific payment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponseModel>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create payments
        public async Task<ActionResult<PaymentResponseModel>> CreatePayment(PaymentRequestModel paymentRequest)
        {
            try
            {
                var payment = await _paymentService.CreatePaymentAsync(paymentRequest);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing payment
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can update payments
        public async Task<ActionResult<PaymentResponseModel>> UpdatePayment(int id, PaymentRequestModel paymentRequest)
        {
            try
            {
                var payment = await _paymentService.UpdatePaymentAsync(id, paymentRequest);
                if (payment == null)
                {
                    return NotFound();
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a payment
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete payments
        public async Task<ActionResult<bool>> DeletePayment(int id)
        {
            var result = await _paymentService.DeletePaymentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private IEnumerable<object> SelectFields(IEnumerable<PaymentResponseModel> payments, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var payment in payments)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("paymentid") || fieldList.Contains("id"))
                    obj["paymentId"] = payment.PaymentId;

                if (fieldList.Contains("orderid"))
                    obj["orderId"] = payment.OrderId;

                if (fieldList.Contains("amount"))
                    obj["amount"] = payment.Amount;

                if (fieldList.Contains("paymentdate"))
                    obj["paymentDate"] = payment.PaymentDate;

                if (fieldList.Contains("paymentmethod"))
                    obj["paymentMethod"] = payment.PaymentMethod;

                yield return obj;
            }
        }
    }
}