using WEX.TransactionAPI.Domain.Exceptions;

namespace WEX.TransactionAPI.Domain.ValueObjects
{
    public record PurchaseDescription
    {
        public const int MaxLength = 50;
        public string Value { get; }

        public PurchaseDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidPurchaseDescriptionException("Description must not be empty.");
            }
            if (value.Length > MaxLength)
            {
                throw new InvalidPurchaseDescriptionException($"Description must not exceed {MaxLength} characters.");
            }
            Value = value;
        }

        // Implicit conversion for convenience
        public static implicit operator string(PurchaseDescription description) => description.Value;
    }
}
