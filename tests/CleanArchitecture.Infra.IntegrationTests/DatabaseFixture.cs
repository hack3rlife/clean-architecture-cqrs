using System;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infra.IntegrationTests
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        private bool _isDisposed;
        public readonly IConfiguration Configuration;

        public BlogDbContext BlogDbContext { get; set; }

        private readonly ILogger<BlogDbContext> _logger;

        /// <summary>
        /// 
        /// </summary>
        public DatabaseFixture()
        {
            var _factory = LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Information));

            _logger = new Logger<BlogDbContext>(_factory);

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            DbContextOptions<BlogDbContext> options;
            if (Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                Console.WriteLine($"# # # # # ConnectionString: UseInMemoryDatabase # # # # #");

                options = new DbContextOptionsBuilder<BlogDbContext>()
                    .UseInMemoryDatabase(databaseName: "inmemblogdb")
                    .Options;
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("BlogDbConnection");
                Console.WriteLine($"* * * * * Database : {connectionString} * * * * *");

                options = new DbContextOptionsBuilder<BlogDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;
            }

            BlogDbContext = new BlogDbContext(options, _logger);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;


            if (disposing)
            {
                BlogDbContext.Dispose();
            }

            _isDisposed = true;
        }

        ~DatabaseFixture()
        {
            Dispose(false);
        }
    }
}