using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResponseModel<IDictionary<string, object?>>>> GetCategories([FromQuery] PagedRequestModel request)
    {
        var result = await _categoryService.GetCategoriesAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IDictionary<string, object?>>> GetCategoryById(int id, [FromQuery] string? select)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id, select);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseModel>> CreateCategory([FromBody] CategoryCreateRequestModel request)
    {
        var created = await _categoryService.CreateCategoryAsync(request);
        return CreatedAtAction(nameof(GetCategoryById), new { id = created.CategoryId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryResponseModel>> UpdateCategory(int id, [FromBody] CategoryUpdateRequestModel request)
    {
        var updated = await _categoryService.UpdateCategoryAsync(id, request);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
