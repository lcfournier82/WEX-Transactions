namespace WEX.TransactionAPI.Domain.Exceptions
{
    [Serializable]
    public class InvalidUsdAmountException : Exception
    {
        public InvalidUsdAmountException()
        {
        }

        public InvalidUsdAmountException(string? message) : base(message)
        {
        }

        public InvalidUsdAmountException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}