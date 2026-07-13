using ToDoApp.DataAccess.Repositories;
using ToDoApp.Domain.Dtos;
using ToDoApp.Domain.Entities;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services;

/// <summary>
/// Implements category business logic. Mirrors TaskService: talks to the
/// repository and maps between entities and DTOs.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(int userId)
    {
        var categories = await _repository.GetAllAsync(userId);
        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponse?> GetCategoryAsync(int id, int userId)
    {
        var category = await _repository.GetByIdAsync(id, userId);
        return category is null ? null : MapToResponse(category);
    }

    public async Task<CategoryResponse> CreateCategoryAsync(int userId, CategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            UserId = userId
        };

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        return MapToResponse(category);
    }

    public async Task<bool> UpdateCategoryAsync(int id, int userId, CategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id, userId);
        if (category is null)
            return false;

        category.Name = request.Name;

        _repository.Update(category);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id, int userId)
    {
        var category = await _repository.GetByIdWithTasksAsync(id, userId);
        if (category is null)
            return false;

        // Step 1: unhook the tasks — they stay, just lose their category.
        // Needed because the Category -> Tasks relationship is Restrict.
        foreach (var task in category.Tasks)
            task.CategoryId = null;

        // Step 2: now the category can be deleted.
        _repository.Remove(category);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static CategoryResponse MapToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };
}
