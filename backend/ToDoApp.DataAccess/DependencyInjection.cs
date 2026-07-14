using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.DataAccess.Persistence;
using ToDoApp.DataAccess.Repositories;

namespace ToDoApp.DataAccess;

/// <summary>
/// Registers everything the data-access layer provides (DbContext, repositories).
/// Keeps Program.cs thin and lets each layer own its own DI wiring.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
