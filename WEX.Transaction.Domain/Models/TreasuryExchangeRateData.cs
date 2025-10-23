using System.Text.Json.Serialization;

namespace WEX.TransactionAPI.Domain.Models
{
    public class TreasuryExchangeRateData
    {
        [JsonPropertyName("exchange_rate")]
        public string ExchangeRate { get; set; } = string.Empty;

        [JsonPropertyName("record_date")]
        public string RecordDate { get; set; } = string.Empty;
    }
}
