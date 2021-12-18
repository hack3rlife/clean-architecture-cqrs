using CleanArchitecture.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CleanArchitecture.WebApi.EndToEndTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(services =>
                {
                    var serviceDescriptor = services.FirstOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<BlogDbContext>));

                    services.Remove(serviceDescriptor);

                })
                .ConfigureTestServices(testServices =>
                {
                    var guid = Guid.NewGuid().ToString();
                    testServices.AddDbContext<BlogDbContext>(c => c.UseInMemoryDatabase(guid).EnableDetailedErrors());

                    IServiceProvider serviceProvider = testServices.BuildServiceProvider();
                    var scope = serviceProvider.CreateScope();
                    var blogDbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

                    BlogDbContextDataSeed.SeedSampleData(blogDbContext);
                });
        }
    }
}