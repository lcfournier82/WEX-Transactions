using MediatR;
using WEX.TransactionAPI.Application.Exceptions;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Domain.Entities;

namespace WEX.TransactionAPI.Application.Purchases.Queries.GetConvertedPurchase
{
    public class GetConvertedPurchaseQueryHandler(
        IPurchaseRepository repository,
        ITreasuryService treasuryService)
        : IRequestHandler<GetConvertedPurchaseQuery, ConvertedPurchaseDto>
    {
        public async Task<ConvertedPurchaseDto> Handle(GetConvertedPurchaseQuery request, CancellationToken cancellationToken)
        {
            // 1. Find the purchase
            var purchase = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Purchase), request.Id);

            // 2. Get the exchange rate
            var (rate, error) = await treasuryService.GetExchangeRateAsync(
                request.TargetCurrency,
                purchase.TransactionDate,
                cancellationToken);

            if (error != null)
            {
                // Requirement #2: Error if no rate is found
                throw new ConversionNotAvailableException(error);
            }

            var exchangeRate = rate.GetValueOrDefault();

            // 3. Calculate and round
            var convertedAmount = Math.Round(purchase.Amount.Value * exchangeRate, 2, MidpointRounding.AwayFromZero);

            // 4. Map to DTO
            return new ConvertedPurchaseDto
            {
                Id = purchase.Id,
                Description = purchase.Description.Value,
                TransactionDate = purchase.TransactionDate,
                OriginalPurchaseAmount = purchase.Amount.Value,
                ExchangeRate = exchangeRate,
                ConvertedAmount = convertedAmount
            };
        }
    }
}
