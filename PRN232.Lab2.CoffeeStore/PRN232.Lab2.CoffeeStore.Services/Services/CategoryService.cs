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
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<CategoryResponseModel>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryResponseModel
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate
            });
        }

        public async Task<CategoryResponseModel?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryResponseModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate
            };
        }

        public async Task<CategoryResponseModel> CreateCategoryAsync(CategoryRequestModel categoryRequest)
        {
            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryResponseModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate
            };
        }

        public async Task<CategoryResponseModel?> UpdateCategoryAsync(int id, CategoryRequestModel categoryRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            category.Name = categoryRequest.Name;
            category.Description = categoryRequest.Description;

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryResponseModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<CategoryResponseModel> Categories, int TotalCount)> GetCategoriesAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null)
        {
            // Get all categories
            var allCategories = (await _unitOfWork.CategoryRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                allCategories = allCategories.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            }

            // Get total count before pagination
            var totalCount = allCategories.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortDirection = ascending ? "asc" : "desc";
                allCategories = allCategories.OrderBy($"{sortBy} {sortDirection}");
            }
            else
            {
                allCategories = allCategories.OrderBy(c => c.CategoryId);
            }

            // Apply pagination
            var categories = allCategories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var categoryResponses = categories.Select(c => new CategoryResponseModel
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate
            });

            return (categoryResponses, totalCount);
        }
    }
}