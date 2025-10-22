using WEX.TransactionAPI.Domain.Exceptions;
using WEX.TransactionAPI.Domain.ValueObjects;

namespace WEX.TransactionAPI.Domain.Entities
{
    // This is our Aggregate Root
    public class Purchase
    {
        public Guid Id { get; private set; }
        public PurchaseDescription Description { get; private set; }
        public DateOnly TransactionDate { get; private set; }
        public UsdAmount Amount { get; private set; }

        // Private constructor for EF Core
        private Purchase() { }

        // Factory method to enforce domain rules on creation
        public static Purchase Create(
            PurchaseDescription description,
            DateOnly transactionDate,
            UsdAmount amount)
        {
            // Any other cross-field validation could go here.
            // For example, if transactions from > 1 year ago were disallowed.

            return new Purchase
            {
                Id = Guid.NewGuid(),
                Description = description,
                TransactionDate = transactionDate,
                Amount = amount
            };
        }
    }
}
