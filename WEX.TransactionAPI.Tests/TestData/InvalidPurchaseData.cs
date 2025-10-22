namespace WEX.TransactionAPI.Tests.TestData
{
    public static class InvalidPurchaseData
    {
        public static IEnumerable<object[]> GetInvalidPurchases()
        {
            yield return new object[] { "", 10.0m, "Description must not be empty." };
            yield return new object[] {
                "This description is way too long, it is over 50 characters for sure.",
                10.0m,
                "Description must not exceed 50 characters."
            };
            yield return new object[] { "Valid Description", -5.00m, "Purchase amount must be a positive value." };
        }

        public static IEnumerable<object[]> GetValidPurchases()
        {
            yield return new object[] { "Laptop", 2500.50m };
            yield return new object[] { "Monitor", 899.90m };
            yield return new object[] { "Keyboard", 199.99m };
        }
    }
}

