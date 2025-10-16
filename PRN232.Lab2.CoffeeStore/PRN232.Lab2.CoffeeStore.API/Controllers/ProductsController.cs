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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseModel>>> GetProducts(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null,
            [FromQuery] int? categoryId = null)
        {
            var (products, totalCount) = await _productService.GetProductsAsync(search, sortBy, ascending, page, pageSize, fields, categoryId);

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
                var selectedProducts = SelectFields(products, fields);
                return Ok(selectedProducts);
            }

            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseModel>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create products
        public async Task<ActionResult<ProductResponseModel>> CreateProduct(ProductRequestModel productRequest)
        {
            try
            {
                var product = await _productService.CreateProductAsync(productRequest);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can update products
        public async Task<ActionResult<ProductResponseModel>> UpdateProduct(int id, ProductRequestModel productRequest)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(id, productRequest);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete products
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private IEnumerable<object> SelectFields(IEnumerable<ProductResponseModel> products, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var product in products)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("productid") || fieldList.Contains("id"))
                    obj["productId"] = product.ProductId;

                if (fieldList.Contains("name"))
                    obj["name"] = product.Name;

                if (fieldList.Contains("description"))
                    obj["description"] = product.Description;

                if (fieldList.Contains("price"))
                    obj["price"] = product.Price;

                if (fieldList.Contains("categoryid"))
                    obj["categoryId"] = product.CategoryId;

                if (fieldList.Contains("isactive"))
                    obj["isActive"] = product.IsActive;

                if (fieldList.Contains("categoryname"))
                    obj["categoryName"] = product.CategoryName;

                yield return obj;
            }
        }
    }
}