using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using PRN232.Lab2.CoffeeStore.Services.Services;
using PRN232.Lab2.CoffeeStore.Repositories.Models;
using System.Linq.Dynamic.Core;

namespace PRN232.Lab2.CoffeeStore.Services.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        public PaymentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<PaymentResponseModel>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();
            return payments.Select(p => new PaymentResponseModel
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod
            });
        }

        public async Task<PaymentResponseModel?> GetPaymentByIdAsync(int id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            return new PaymentResponseModel
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod
            };
        }

        public async Task<PaymentResponseModel> CreatePaymentAsync(PaymentRequestModel paymentRequest)
        {
            var payment = new Payment
            {
                OrderId = paymentRequest.OrderId,
                Amount = paymentRequest.Amount,
                PaymentDate = paymentRequest.PaymentDate ?? DateTime.Now,
                PaymentMethod = paymentRequest.PaymentMethod
            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentResponseModel
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod
            };
        }

        public async Task<PaymentResponseModel?> UpdatePaymentAsync(int id, PaymentRequestModel paymentRequest)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            payment.OrderId = paymentRequest.OrderId;
            payment.Amount = paymentRequest.Amount;
            payment.PaymentDate = paymentRequest.PaymentDate ?? payment.PaymentDate;
            payment.PaymentMethod = paymentRequest.PaymentMethod;

            _unitOfWork.PaymentRepository.Update(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentResponseModel
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod
            };
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null) return false;

            _unitOfWork.PaymentRepository.Remove(payment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<PaymentResponseModel> Payments, int TotalCount)> GetPaymentsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? orderId = null)
        {
            // Get all payments
            var allPayments = (await _unitOfWork.PaymentRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                allPayments = allPayments.Where(p => p.PaymentMethod.Contains(search));
            }

            // Apply order filter
            if (orderId.HasValue)
            {
                allPayments = allPayments.Where(p => p.OrderId == orderId);
            }

            // Get total count before pagination
            var totalCount = allPayments.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortDirection = ascending ? "asc" : "desc";
                allPayments = allPayments.OrderBy($"{sortBy} {sortDirection}");
            }
            else
            {
                allPayments = allPayments.OrderBy(p => p.PaymentId);
            }

            // Apply pagination
            var payments = allPayments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var paymentResponses = payments.Select(p => new PaymentResponseModel
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod
            });

            return (paymentResponses, totalCount);
        }
    }
}