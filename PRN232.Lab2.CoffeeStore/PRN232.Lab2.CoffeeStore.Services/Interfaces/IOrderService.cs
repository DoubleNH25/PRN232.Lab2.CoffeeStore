using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces;

public interface IOrderService
{
    Task<PagedResponseModel<IDictionary<string, object?>>> GetOrdersAsync(PagedRequestModel request);
    Task<IDictionary<string, object?>?> GetOrderByIdAsync(int orderId, string? select);
    Task<OrderResponseModel> CreateOrderAsync(OrderCreateRequestModel request);
    Task<OrderResponseModel?> UpdateOrderAsync(int orderId, OrderUpdateRequestModel request);
    Task<bool> DeleteOrderAsync(int orderId);
}
