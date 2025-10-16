using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseModel>> GetAllPaymentsAsync();
        Task<PaymentResponseModel?> GetPaymentByIdAsync(int id);
        Task<PaymentResponseModel> CreatePaymentAsync(PaymentRequestModel paymentRequest);
        Task<PaymentResponseModel?> UpdatePaymentAsync(int id, PaymentRequestModel paymentRequest);
        Task<bool> DeletePaymentAsync(int id);
        Task<(IEnumerable<PaymentResponseModel> Payments, int TotalCount)> GetPaymentsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? orderId = null);
    }
}