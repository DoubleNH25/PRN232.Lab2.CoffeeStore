using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using PRN232.Lab2.CoffeeStore.Services.Services;
using PRN232.Lab2.CoffeeStore.Repositories.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PRN232.Lab2.CoffeeStore.Services.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<ProductResponseModel>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.CategoryId, c => c.Name);

            return products.Select(p => new ProductResponseModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                CategoryName = p.CategoryId.HasValue && categoryDict.ContainsKey(p.CategoryId.Value) 
                    ? categoryDict[p.CategoryId.Value] : null
            });
        }

        public async Task<ProductResponseModel?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return null;

            string? categoryName = null;
            if (product.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId.Value);
                categoryName = category?.Name;
            }

            return new ProductResponseModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                CategoryName = categoryName
            };
        }

        public async Task<ProductResponseModel> CreateProductAsync(ProductRequestModel productRequest)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId,
                IsActive = productRequest.IsActive ?? true
            };

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            string? categoryName = null;
            if (product.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId.Value);
                categoryName = category?.Name;
            }

            return new ProductResponseModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                CategoryName = categoryName
            };
        }

        public async Task<ProductResponseModel?> UpdateProductAsync(int id, ProductRequestModel productRequest)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = productRequest.Name;
            product.Description = productRequest.Description;
            product.Price = productRequest.Price;
            product.CategoryId = productRequest.CategoryId;
            product.IsActive = productRequest.IsActive ?? product.IsActive;

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            string? categoryName = null;
            if (product.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId.Value);
                categoryName = category?.Name;
            }

            return new ProductResponseModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                CategoryName = categoryName
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.ProductRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<ProductResponseModel> Products, int TotalCount)> GetProductsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? categoryId = null)
        {
            // Get all products
            var allProducts = (await _unitOfWork.ProductRepository.GetAllAsync()).AsQueryable();
            
            // Include categories for category name
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.CategoryId, c => c.Name);

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            // Apply category filter
            if (categoryId.HasValue)
            {
                allProducts = allProducts.Where(p => p.CategoryId == categoryId);
            }

            // Get total count before pagination
            var totalCount = allProducts.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Handle special case for CategoryName
                if (sortBy.Equals("categoryName", StringComparison.OrdinalIgnoreCase))
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allProducts = allProducts.OrderBy(p => categoryDict.ContainsKey(p.CategoryId ?? 0) ? categoryDict[p.CategoryId ?? 0] : "").ThenBy(p => p.ProductId);
                    if (!ascending)
                    {
                        allProducts = allProducts.Reverse();
                    }
                }
                else
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allProducts = allProducts.OrderBy($"{sortBy} {sortDirection}");
                }
            }
            else
            {
                allProducts = allProducts.OrderBy(p => p.ProductId);
            }

            // Apply pagination
            var products = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var productResponses = products.Select(p => new ProductResponseModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                CategoryName = p.CategoryId.HasValue && categoryDict.ContainsKey(p.CategoryId.Value) 
                    ? categoryDict[p.CategoryId.Value] : null
            });

            return (productResponses, totalCount);
        }
    }
}