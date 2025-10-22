using WEX.TransactionAPI.Domain.Exceptions;

namespace WEX.TransactionAPI.Domain.ValueObjects
{
    public record UsdAmount
    {
        public decimal Value { get; }

        public UsdAmount(decimal value)
        {
            if (value <= 0)
            {
                throw new InvalidUsdAmountException("Purchase amount must be a positive value.");
            }

            // Requirement #1: Round to nearest cent
            Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        // Implicit conversion for convenience
        public static implicit operator decimal(UsdAmount amount) => amount.Value;
    }
}
