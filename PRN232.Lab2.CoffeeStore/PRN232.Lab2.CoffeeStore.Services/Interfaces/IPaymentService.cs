using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces;

public interface IPaymentService
{
    Task<PagedResponseModel<IDictionary<string, object?>>> GetPaymentsAsync(PagedRequestModel request);
    Task<IDictionary<string, object?>?> GetPaymentByIdAsync(int paymentId, string? select);
    Task<PaymentResponseModel> CreatePaymentAsync(int orderId, PaymentCreateRequestModel request);
    Task<PaymentResponseModel?> UpdatePaymentAsync(int paymentId, PaymentUpdateRequestModel request);
    Task<bool> DeletePaymentAsync(int paymentId);
}
