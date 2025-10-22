using MediatR;

namespace WEX.TransactionAPI.Application.Purchases.Queries.GetConvertedPurchase
{
    public record GetConvertedPurchaseQuery(
        Guid Id,
        string TargetCurrency) : IRequest<ConvertedPurchaseDto>;
}
