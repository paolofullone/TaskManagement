using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Context;
using WebApi.Infrastructure.GraphQL;

namespace WebApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MVC/API Services
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Database
        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });


        // GraphQL
        services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .RegisterDbContextFactory<ApplicationDbContext>();

        return services;
    }
}
