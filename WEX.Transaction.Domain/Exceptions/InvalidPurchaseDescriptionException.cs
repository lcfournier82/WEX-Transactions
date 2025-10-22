namespace WEX.TransactionAPI.Domain.Exceptions
{
    [Serializable]
    public class InvalidPurchaseDescriptionException : Exception
    {
        public InvalidPurchaseDescriptionException()
        {
        }

        public InvalidPurchaseDescriptionException(string? message) : base(message)
        {
        }

        public InvalidPurchaseDescriptionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}