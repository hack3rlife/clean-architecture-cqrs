using CleanArchitecture.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using CleanArchitecture.WebApi.EndToEndTests.Infrastructure;

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
                    testServices.AddSingleton<IEndToEndClient, EndToEndClient>();
                    testServices.AddTransient<LoggingHandler>();

                    testServices.AddSingleton<EndToEndClientFactory>();
                    testServices.AddSingleton<BlogServiceClient>();

                    testServices
                        .AddHttpClient<EndToEndClient>()
                        .ConfigureHttpMessageHandlerBuilder(x =>
                        {
                            //attach testserver handler to the httpclient so request doesn't fail when running locally
                            //https://lurumad.github.io/integration-tests-in-aspnet-core-signalr
#if DEBUG
                            x.PrimaryHandler = Server.CreateHandler();
#endif
                        })
                        .AddHttpMessageHandler<LoggingHandler>();

                    var guid = Guid.NewGuid().ToString();
                    testServices.AddDbContext<BlogDbContext>(c => c.UseInMemoryDatabase(guid).EnableDetailedErrors());

                    var serviceProvider = testServices.BuildServiceProvider();
                    var scope = serviceProvider.CreateScope();
                    var blogDbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

                    BlogDbContextDataSeed.SeedSampleData(blogDbContext);
                });
        }
    }
}