using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Context;
using WebApi.Models;

namespace WebApi.Infrastructure.GraphQL;

public class Query
{
    // Basic queries
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetUsers([Service] ApplicationDbContext context)
        => context.Users;

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Project> GetProjects([Service] ApplicationDbContext context)
        => context.Projects;

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WorkTask> GetWorkTasks([Service] ApplicationDbContext context)
        => context.WorkTasks;

    // Single entity by ID
    public async Task<User?> GetUserById(
        [Service] ApplicationDbContext context,
        int id)
        => await context.Users
            .Include(u => u.Tasks)
            .ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Project?> GetProjectById(
        [Service] ApplicationDbContext context,
        int id)
        => await context.Projects
            .Include(p => p.Tasks)
            .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<WorkTask?> GetWorkTaskById(
        [Service] ApplicationDbContext context,
        int id)
        => await context.WorkTasks
            .Include(t => t.User)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);

    // Filtered queries
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WorkTask> GetCompletedTasks(
        [Service] ApplicationDbContext context,
        bool isCompleted)
        => context.WorkTasks
            .Where(t => t.IsCompleted == isCompleted)
            .Include(t => t.User)
            .Include(t => t.Project);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WorkTask> GetTasksByUser(
        [Service] ApplicationDbContext context,
        int userId)
        => context.WorkTasks
            .Where(t => t.UserId == userId)
            .Include(t => t.Project);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WorkTask> GetFilteredTasksByUser(
    [Service] ApplicationDbContext context,
    int userId, bool isCompleted)
        => context.WorkTasks
        .Where(t => t.UserId == userId && t.IsCompleted == isCompleted)
        .Include(t => t.Project);
}