using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Persistence;

/// <summary>
/// EF Core database context — the bridge between our entities and the SQL database.
/// Each DbSet becomes a table.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Username).IsUnique(); // no two users with the same login
            entity.Property(u => u.PasswordHash).IsRequired();

            // Deleting a user deletes their tasks and categories.
            entity.HasMany(u => u.Tasks)
                  .WithOne(t => t.User!)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Categories)
                  .WithOne(c => c.User!)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

            // Restrict (not SetNull/Cascade) to avoid SQL Server's "multiple cascade
            // paths" error: Tasks are already reachable from User via a cascade path.
            // When a category is deleted we detach its tasks (set CategoryId = null)
            // manually in the service layer before removing the category.
            entity.HasMany(c => c.Tasks)
                  .WithOne(t => t.Category!)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(1000);
        });

        // TEMPORARY: a seed user so tasks can be created before authentication
        // exists. The real registration flow (auth step) will add proper users.
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "seeduser",
            PasswordHash = "SEED_PLACEHOLDER",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
