using MediatR;
using WEX.TransactionAPI.Domain.ValueObjects;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Domain.Entities;

namespace WEX.TransactionAPI.Application.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommandHandler(
        IPurchaseRepository repository,
        IApplicationDbContext context)
        : IRequestHandler<CreatePurchaseCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Value Objects (this enforces domain rules)
            var description = new PurchaseDescription(request.Description);
            var amount = new UsdAmount(request.PurchaseAmount);

            // 2. Create the Entity using the factory method
            var purchase = Purchase.Create(
                description,
                request.TransactionDate,
                amount
            );

            // 3. Persist
            await repository.AddAsync(purchase, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // 4. Return the new ID
            return purchase.Id;
        }
    }
}
