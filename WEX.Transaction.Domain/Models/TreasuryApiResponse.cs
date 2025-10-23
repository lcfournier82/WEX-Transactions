using System.Text.Json.Serialization;

namespace WEX.TransactionAPI.Domain.Models
{
    public class TreasuryApiResponse
    {
        [JsonPropertyName("data")]
        public List<TreasuryExchangeRateData> Data { get; set; } = [];
    }
}
