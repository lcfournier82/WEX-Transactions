using MediatR;
using Microsoft.AspNetCore.Mvc;
using WEX.TransactionAPI.Application.Purchases.Commands.CreatePurchase;
using WEX.TransactionAPI.Application.Purchases.Queries.GetConvertedPurchase;

namespace wex_transactions_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PurchasesController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// Stores a new purchase transaction.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> StorePurchase([FromBody] CreatePurchaseCommand command)
        {
            var purchaseId = await sender.Send(command);

            // We return a 201 Created with the ID. 
            // A GET /api/purchases/{id} endpoint could be added.
            return CreatedAtAction(nameof(StorePurchase), new { id = purchaseId }, purchaseId);
        }

        /// <summary>
        /// Retrieves a purchase transaction converted to a specified currency.
        /// </summary>
        [HttpGet("{id}/convert")]
        [ProducesResponseType(typeof(ConvertedPurchaseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetConvertedPurchase(
            [FromRoute] Guid id,
            [FromQuery] string targetCurrency)
        {
            var query = new GetConvertedPurchaseQuery(id, targetCurrency);
            var result = await sender.Send(query);
            return Ok(result);
        }
    }
}
