using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using WEX.TransactionAPI.Application.Purchases.Commands;
using WEX.TransactionAPI.Application.Purchases.Commands.CreatePurchase;
using WEX.TransactionAPI.Tests.TestData;
using WEX.TransactionAPI.Tests.Testing;
using Xunit;

namespace WEX.TransactionAPI.Tests.Integration
{
    public class PurchasesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PurchasesControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [MemberData(nameof(InvalidPurchaseData.GetInvalidPurchases), MemberType = typeof(InvalidPurchaseData))]
        public async Task StorePurchase_WithInvalidData_ReturnsBadRequest(string description, decimal amount, string expectedError)
        {
            // Arrange
            var command = new CreatePurchaseCommand(description, new DateOnly(2025, 10, 20), amount);

            // Act
            var response = await _client.PostAsJsonAsync("/api/purchases", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problem!.Detail.Should().Be(expectedError);
        }

        [Theory]
        [MemberData(nameof(InvalidPurchaseData.GetValidPurchases), MemberType = typeof(InvalidPurchaseData))]
        public async Task StorePurchase_WithValidData_ReturnsCreated(string description, decimal amount)
        {
            var command = new CreatePurchaseCommand(description, new DateOnly(2025, 10, 21), amount);

            var response = await _client.PostAsJsonAsync("/api/purchases", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
