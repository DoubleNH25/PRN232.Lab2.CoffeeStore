using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResponseModel<IDictionary<string, object?>>>> GetProducts([FromQuery] PagedRequestModel request)
    {
        var result = await _productService.GetProductsAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IDictionary<string, object?>>> GetProductById(int id, [FromQuery] string? select)
    {
        var result = await _productService.GetProductByIdAsync(id, select);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ProductResponseModel>> CreateProduct([FromBody] ProductCreateRequestModel request)
    {
        var created = await _productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ProductResponseModel>> UpdateProduct(int id, [FromBody] ProductUpdateRequestModel request)
    {
        var updated = await _productService.UpdateProductAsync(id, request);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
