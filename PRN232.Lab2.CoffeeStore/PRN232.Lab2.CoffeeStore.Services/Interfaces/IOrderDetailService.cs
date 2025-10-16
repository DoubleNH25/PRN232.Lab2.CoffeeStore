using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseModel>> GetAllOrderDetailsAsync();
        Task<OrderDetailResponseModel?> GetOrderDetailByIdAsync(int id);
        Task<OrderDetailResponseModel> CreateOrderDetailAsync(OrderDetailRequestModel orderDetailRequest);
        Task<OrderDetailResponseModel?> UpdateOrderDetailAsync(int id, OrderDetailRequestModel orderDetailRequest);
        Task<bool> DeleteOrderDetailAsync(int id);
        Task<(IEnumerable<OrderDetailResponseModel> OrderDetails, int TotalCount)> GetOrderDetailsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? orderId = null);
    }
}