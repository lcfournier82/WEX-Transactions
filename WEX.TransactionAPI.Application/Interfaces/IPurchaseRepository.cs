using WEX.TransactionAPI.Domain.Entities;

namespace WEX.TransactionAPI.Application.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Purchase purchase, CancellationToken cancellationToken);
    }
}
