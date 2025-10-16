using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseModel>> GetAllCategoriesAsync();
        Task<CategoryResponseModel?> GetCategoryByIdAsync(int id);
        Task<CategoryResponseModel> CreateCategoryAsync(CategoryRequestModel categoryRequest);
        Task<CategoryResponseModel?> UpdateCategoryAsync(int id, CategoryRequestModel categoryRequest);
        Task<bool> DeleteCategoryAsync(int id);
        Task<(IEnumerable<CategoryResponseModel> Categories, int TotalCount)> GetCategoriesAsync(
            string? search = null, 
            string? sortBy = null, 
            bool ascending = true, 
            int page = 1, 
            int pageSize = 10,
            string? fields = null);
    }
}