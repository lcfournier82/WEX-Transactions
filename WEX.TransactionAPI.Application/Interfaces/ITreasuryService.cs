namespace WEX.TransactionAPI.Application.Interfaces
{
    public interface ITreasuryService
    {
        Task<(decimal? Rate, string? Error)> GetExchangeRateAsync(string targetCurrency, DateOnly purchaseDate, CancellationToken cancellationToken);
    }
}
