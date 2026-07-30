using Microsoft.EntityFrameworkCore;
using todo_api.Models;

namespace todo_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Todo>().HasData(
                new Todo { Id = 1, Title = "Sample Todo 1", IsCompleted = false, CompletedAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Todo { Id = 2, Title = "Sample Todo 2", IsCompleted = true, CompletedAt = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}
 