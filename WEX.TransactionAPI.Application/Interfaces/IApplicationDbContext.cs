using Microsoft.EntityFrameworkCore;
using WEX.TransactionAPI.Domain.Entities;

namespace WEX.TransactionAPI.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Purchase> Purchases { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
