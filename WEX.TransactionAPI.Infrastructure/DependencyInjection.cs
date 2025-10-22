using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Application.Services;
using WEX.TransactionAPI.Infrastructure.Persistence;
using WEX.TransactionAPI.Infrastructure.Persistence.Repositories;

namespace WEX.TransactionAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Configure PostgreSQL DbContext
            services.AddDbContext<IApplicationDbContext, PurchaseDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(PurchaseDbContext).Assembly.FullName)));

            // 2. Register Repositories
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();

            // 3. Register Treasury API Client
            services.AddHttpClient<ITreasuryService, TreasuryService>(client =>
            {
                client.BaseAddress = new Uri(configuration["TreasuryApi:BaseUrl"]
                    ?? throw new InvalidOperationException("TreasuryApi:BaseUrl is not configured."));
            });

            return services;
        }
    }
}
