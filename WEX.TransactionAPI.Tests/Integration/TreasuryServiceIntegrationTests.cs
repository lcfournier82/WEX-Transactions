using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Application.Services;
using Xunit;

namespace WEX.TransactionAPI.Tests.Integration
{
    public class TreasuryServiceIntegrationTests
    {
        private readonly ITreasuryService _treasuryService;

        public TreasuryServiceIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var logger = LoggerFactory.Create(builder => builder.AddConsole())
                                      .CreateLogger<TreasuryService>();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["TreasuryApi:BaseUrl"] ??
                    throw new InvalidOperationException("TreasuryApi:BaseUrl is not configured."))
            };

            _treasuryService = new TreasuryService(httpClient, configuration, logger);
        }

        [Fact]
        public async Task GetExchangeRateAsync_RealService_ReturnsPositiveRate()
        {
            // Arrange
            string targetCurrency = "Canada-Dollar";
            var purchaseDate = DateOnly.FromDateTime(new DateTime(2025,09,30));

            // Act
            var (rateValue, currency) = await _treasuryService.GetExchangeRateAsync(targetCurrency, purchaseDate, CancellationToken.None);

            // Assert
            rateValue.Should().BeGreaterThan(0);
        }
    }
}
