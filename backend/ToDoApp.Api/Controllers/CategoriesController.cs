using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Domain.Dtos;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Authorize] // every endpoint here requires a valid JWT
[Route("api/[controller]")] // -> /api/categories
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // The authenticated user's id, read from the JWT (the NameIdentifier claim we put in the token).
    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var categories = await _categoryService.GetCategoriesAsync(CurrentUserId);
        return Ok(categories);
    }

    // GET /api/categories/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryAsync(id, CurrentUserId);
        return category is null ? NotFound() : Ok(category);
    }

    // POST /api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryRequest request)
    {
        var created = await _categoryService.CreateCategoryAsync(CurrentUserId, request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequest request)
    {
        var updated = await _categoryService.UpdateCategoryAsync(id, CurrentUserId, request);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id, CurrentUserId);
        return deleted ? NoContent() : NotFound();
    }
}
