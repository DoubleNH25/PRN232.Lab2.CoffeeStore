using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces;

public interface ICategoryService
{
    Task<PagedResponseModel<IDictionary<string, object?>>> GetCategoriesAsync(PagedRequestModel request);
    Task<IDictionary<string, object?>?> GetCategoryByIdAsync(int categoryId, string? select);
    Task<CategoryResponseModel> CreateCategoryAsync(CategoryCreateRequestModel request);
    Task<CategoryResponseModel?> UpdateCategoryAsync(int categoryId, CategoryUpdateRequestModel request);
    Task<bool> DeleteCategoryAsync(int categoryId);
}
