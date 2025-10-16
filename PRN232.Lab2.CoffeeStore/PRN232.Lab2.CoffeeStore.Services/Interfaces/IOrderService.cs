using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetAllOrdersAsync();
        Task<OrderResponseModel?> GetOrderByIdAsync(int id);
        Task<OrderResponseModel> CreateOrderAsync(OrderRequestModel orderRequest);
        Task<OrderResponseModel?> UpdateOrderAsync(int id, OrderRequestModel orderRequest);
        Task<bool> DeleteOrderAsync(int id);
        Task<(IEnumerable<OrderResponseModel> Orders, int TotalCount)> GetOrdersAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? userId = null);
    }
}