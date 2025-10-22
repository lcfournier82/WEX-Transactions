using MediatR;

namespace WEX.TransactionAPI.Application.Purchases.Commands.CreatePurchase
{
    public record CreatePurchaseCommand(
        string Description,
        DateOnly TransactionDate,
        decimal PurchaseAmount) : IRequest<Guid>;
}
