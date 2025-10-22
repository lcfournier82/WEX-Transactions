using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEX.TransactionAPI.Application.Exceptions;
using WEX.TransactionAPI.Domain.Exceptions;

namespace wex_transactions_api.Infrastructure
{
    // Handles custom exceptions and maps them to HTTP responses
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred.");

            var (statusCode, title, detail) = exception switch
            {
                // Domain Exceptions
                InvalidPurchaseDescriptionException or InvalidUsdAmountException
                    => (StatusCodes.Status400BadRequest, "Validation Error", exception.Message),

                // Application Exceptions
                NotFoundException
                    => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
                ConversionNotAvailableException
                    => (StatusCodes.Status400BadRequest, "Conversion Error", exception.Message),
                FluentValidation.ValidationException validationException
                    => (StatusCodes.Status400BadRequest, "Validation Error", validationException.Errors.First().ErrorMessage),

                // Fallback
                _ => (StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occurred.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
