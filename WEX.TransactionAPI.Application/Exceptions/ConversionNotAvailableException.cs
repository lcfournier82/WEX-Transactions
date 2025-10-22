namespace WEX.TransactionAPI.Application.Exceptions
{
    [Serializable]
    public class ConversionNotAvailableException : Exception
    {
        private object error;

        public ConversionNotAvailableException()
        {
        }

        public ConversionNotAvailableException(object error)
        {
            this.error = error;
        }

        public ConversionNotAvailableException(string? message) : base(message)
        {
        }

        public ConversionNotAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}