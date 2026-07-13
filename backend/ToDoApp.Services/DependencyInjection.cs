using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services;

/// <summary>
/// Registers the business-logic services. Called from the API's Program.cs.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
