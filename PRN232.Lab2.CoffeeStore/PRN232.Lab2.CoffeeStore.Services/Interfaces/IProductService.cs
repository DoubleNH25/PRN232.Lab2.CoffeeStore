using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces;

public interface IProductService
{
    Task<PagedResponseModel<IDictionary<string, object?>>> GetProductsAsync(PagedRequestModel request);
    Task<IDictionary<string, object?>?> GetProductByIdAsync(int productId, string? select);
    Task<ProductResponseModel> CreateProductAsync(ProductCreateRequestModel request);
    Task<ProductResponseModel?> UpdateProductAsync(int productId, ProductUpdateRequestModel request);
    Task<bool> DeleteProductAsync(int productId);
}
