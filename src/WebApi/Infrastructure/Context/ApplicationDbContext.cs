using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<WorkTask> WorkTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>().ToTable("USERS");
        modelBuilder.Entity<Project>().ToTable("PROJECTS");
        modelBuilder.Entity<WorkTask>().ToTable("WORKTASKS");

        // Configure relationships
        modelBuilder.Entity<WorkTask>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<WorkTask>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

    }
}
