using Microsoft.EntityFrameworkCore;
using Models.TaskItem;

public class AppDbContext : DbContext
{


    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
}
