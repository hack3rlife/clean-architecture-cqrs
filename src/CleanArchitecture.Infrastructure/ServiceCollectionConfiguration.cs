using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
    public static class ServiceCollectionConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<BlogDbContext>(options => options.UseInMemoryDatabase(databaseName: "inmemblogdb"));
            }
            else
            {
                // SQL Server Database Connection
                var connectionString = configuration.GetConnectionString("BlogDbConnection");
                services.AddDbContext<BlogDbContext>(c => c.UseSqlServer(connectionString));
            }
            return services;
        }
    }
}