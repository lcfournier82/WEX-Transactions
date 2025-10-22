using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Npgsql;
using Respawn;
using RichardSzalay.MockHttp;
using Testcontainers.PostgreSql;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Infrastructure.Persistence;
using Xunit;

namespace WEX.TransactionAPI.Tests.Testing
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("purchases_db")
            .WithUsername("wex_user")
            .WithPassword("wexApiTest@01235")
            .Build();

        public MockHttpMessageHandler MockHttp { get; } = new();
        private DbConnection _dbConnection = null!;
        private Respawner _respawner = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. Remove the real DbContext configuration
                services.RemoveAll<DbContextOptions<PurchaseDbContext>>();
                services.RemoveAll<IApplicationDbContext>();

                // 2. Add DbContext pointing to the test container
                services.AddDbContext<IApplicationDbContext, PurchaseDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                // 3. Replace IHttpClientFactory with the MockHttp handler
                services.RemoveAll<IHttpClientFactory>();
                services.AddSingleton<IHttpClientFactory>(sp =>
                {
                    var factoryMock = new Mock<IHttpClientFactory>();
                    factoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                               .Returns(MockHttp.ToHttpClient());
                    return factoryMock.Object;
                });
            });
        }

        public async Task InitializeAsync()
        {
            // Start the Docker container
            await _dbContainer.StartAsync();

            // Apply migrations to the test database
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PurchaseDbContext>();
            await context.Database.MigrateAsync();

            // Setup Respawner to clean the DB
            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            });
        }

        public async Task ResetDatabaseAsync()
        {
            // Clean all data from tables
            await _respawner.ResetAsync(_dbConnection);
            MockHttp.Dispose();
        }

        public new async Task DisposeAsync()
        {
            await _dbConnection.CloseAsync();
            await _dbContainer.StopAsync();
            await base.DisposeAsync();
        }
    }
}
