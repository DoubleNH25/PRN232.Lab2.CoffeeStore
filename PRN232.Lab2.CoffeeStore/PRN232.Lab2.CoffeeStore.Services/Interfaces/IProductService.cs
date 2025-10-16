using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseModel>> GetAllProductsAsync();
        Task<ProductResponseModel?> GetProductByIdAsync(int id);
        Task<ProductResponseModel> CreateProductAsync(ProductRequestModel productRequest);
        Task<ProductResponseModel?> UpdateProductAsync(int id, ProductRequestModel productRequest);
        Task<bool> DeleteProductAsync(int id);
        Task<(IEnumerable<ProductResponseModel> Products, int TotalCount)> GetProductsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? categoryId = null);
    }
}