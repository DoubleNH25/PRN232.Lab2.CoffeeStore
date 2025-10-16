using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using System.Text.Json;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOrStaff")] // Require authentication for all endpoints
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all categories with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetCategories(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null)
        {
            var (categories, totalCount) = await _categoryService.GetCategoriesAsync(search, sortBy, ascending, page, pageSize, fields);

            // Add pagination headers
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            }));

            // Apply field selection if specified
            if (!string.IsNullOrEmpty(fields))
            {
                var selectedCategories = SelectFields(categories, fields);
                return Ok(selectedCategories);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Get a specific category by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseModel>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create categories
        public async Task<ActionResult<CategoryResponseModel>> CreateCategory(CategoryRequestModel categoryRequest)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(categoryRequest);
                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can update categories
        public async Task<ActionResult<CategoryResponseModel>> UpdateCategory(int id, CategoryRequestModel categoryRequest)
        {
            try
            {
                var category = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete categories
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private IEnumerable<object> SelectFields(IEnumerable<CategoryResponseModel> categories, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var category in categories)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("categoryid") || fieldList.Contains("id"))
                    obj["categoryId"] = category.CategoryId;

                if (fieldList.Contains("name"))
                    obj["name"] = category.Name;

                if (fieldList.Contains("description"))
                    obj["description"] = category.Description;

                if (fieldList.Contains("createddate"))
                    obj["createdDate"] = category.CreatedDate;

                yield return obj;
            }
        }
    }
}