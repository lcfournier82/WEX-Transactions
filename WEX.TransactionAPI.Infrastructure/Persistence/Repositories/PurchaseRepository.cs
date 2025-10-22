using Microsoft.EntityFrameworkCore;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Domain.Entities;

namespace WEX.TransactionAPI.Infrastructure.Persistence.Repositories
{
    public class PurchaseRepository(IApplicationDbContext context) : IPurchaseRepository
    {
        public async Task<Purchase?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            await context.Purchases.AddAsync(purchase, cancellationToken);
        }
    }
}
