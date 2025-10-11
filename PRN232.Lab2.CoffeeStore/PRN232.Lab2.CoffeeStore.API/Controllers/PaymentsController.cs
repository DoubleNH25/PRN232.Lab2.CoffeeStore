using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.API.Controllers;

[ApiController]
[Route("api/orders/{orderId:int}/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseModel<IDictionary<string, object?>>>> GetPayments(int orderId, [FromQuery] PagedRequestModel request)
    {
        request.Search ??= orderId.ToString();
        var result = await _paymentService.GetPaymentsAsync(request);
        return Ok(result);
    }

    [HttpGet("{paymentId:int}")]
    public async Task<ActionResult<IDictionary<string, object?>>> GetPaymentById(int orderId, int paymentId, [FromQuery] string? select)
    {
        var result = await _paymentService.GetPaymentByIdAsync(paymentId, select);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponseModel>> CreatePayment(int orderId, [FromBody] PaymentCreateRequestModel request)
    {
        var created = await _paymentService.CreatePaymentAsync(orderId, request);
        return CreatedAtAction(nameof(GetPaymentById), new { orderId, paymentId = created.PaymentId }, created);
    }

    [HttpPut("{paymentId:int}")]
    public async Task<ActionResult<PaymentResponseModel>> UpdatePayment(int orderId, int paymentId, [FromBody] PaymentUpdateRequestModel request)
    {
        if (paymentId != request.PaymentId)
        {
            return BadRequest("Payment ID mismatch");
        }

        var updated = await _paymentService.UpdatePaymentAsync(paymentId, request);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{paymentId:int}")]
    public async Task<IActionResult> DeletePayment(int orderId, int paymentId)
    {
        var deleted = await _paymentService.DeletePaymentAsync(paymentId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
