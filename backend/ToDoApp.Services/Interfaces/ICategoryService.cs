using ToDoApp.Domain.Dtos;

namespace ToDoApp.Services.Interfaces;

/// <summary>
/// Business operations for categories. Controllers depend on this interface.
/// (DeleteCategoryAsync is added in the next step — it needs special handling.)
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(int userId);
    Task<CategoryResponse?> GetCategoryAsync(int id, int userId);
    Task<CategoryResponse> CreateCategoryAsync(int userId, CategoryRequest request);
    Task<bool> UpdateCategoryAsync(int id, int userId, CategoryRequest request);
    Task<bool> DeleteCategoryAsync(int id, int userId);
}
