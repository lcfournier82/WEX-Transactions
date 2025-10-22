using FluentValidation;

namespace WEX.TransactionAPI.Application.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
    {
        public CreatePurchaseCommandValidator()
        {
            // These are checks on the raw request *before*
            // they become domain value objects.
            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(50).WithMessage("Description must not exceed 50 characters.");

            RuleFor(v => v.PurchaseAmount)
                .GreaterThan(0).WithMessage("Purchase amount must be positive.");
        }
    }
}
