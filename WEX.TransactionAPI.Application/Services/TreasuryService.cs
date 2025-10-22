using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using WEX.TransactionAPI.Application.Interfaces;

namespace WEX.TransactionAPI.Application.Services
{
    // --- DTOs for deserializing the Treasury API Response ---
    public class TreasuryApiResponse
    {
        [JsonPropertyName("data")]
        public List<TreasuryExchangeRateData> Data { get; set; } = [];
    }

    public class TreasuryExchangeRateData
    {
        [JsonPropertyName("exchange_rate")]
        public string ExchangeRate { get; set; } = string.Empty;

        [JsonPropertyName("record_date")]
        public string RecordDate { get; set; } = string.Empty;
    }
    // --- End of DTOs ---

    public class TreasuryService(HttpClient httpClient, IConfiguration configuration, ILogger<TreasuryService> logger) : ITreasuryService
    {
        private readonly string _baseUrl = configuration["TreasuryApi:BaseUrl"]
            ?? throw new InvalidOperationException("TreasuryApi:BaseUrl is not configured.");

        public async Task<(decimal? Rate, string? Error)> GetExchangeRateAsync(string targetCurrency, DateOnly purchaseDate, CancellationToken cancellationToken)
        {
            // Calculate the 6-month lookback date
            var sixMonthsAgo = purchaseDate.AddMonths(-6);

            // Format dates for the API query
            var purchaseDateStr = purchaseDate.ToString("yyyy-MM-dd");
            var sixMonthsAgoStr = sixMonthsAgo.ToString("yyyy-MM-dd");

            // Build the query URL
            var fields = "fields=exchange_rate,record_date";
            var filter = $"filter=country_currency_desc:eq:{targetCurrency},record_date:lte:{purchaseDateStr},record_date:gte:{sixMonthsAgoStr}";
            var sort = "sort=-record_date"; // Get the most recent rate in the window
            var pagination = "page[size]=1";

            var requestUrl = $"{_baseUrl}?{fields}&{filter}&{sort}&{pagination}";

            try
            {
                var response = await httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Failed to fetch exchange rate. Status: {StatusCode}, URL: {RequestUrl}", response.StatusCode, requestUrl);
                    return (null, "Failed to communicate with the Treasury API.");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                var apiData = JsonSerializer.Deserialize<TreasuryApiResponse>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var rateData = apiData?.Data.FirstOrDefault();

                if (rateData == null || !decimal.TryParse(rateData.ExchangeRate, out var rate))
                {
                    // Requirement #2: Error if no rate is available
                    logger.LogWarning("No exchange rate found for {TargetCurrency} on or before {PurchaseDate} within 6 months.", targetCurrency, purchaseDate);
                    return (null, $"The purchase cannot be converted to the target currency. No exchange rate available for {targetCurrency} within 6 months prior to {purchaseDateStr}.");
                }

                return (rate, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error calling Treasury API for {TargetCurrency}", targetCurrency);
                return (null, "An unexpected error occurred while fetching the exchange rate.");
            }
        }
    }
}