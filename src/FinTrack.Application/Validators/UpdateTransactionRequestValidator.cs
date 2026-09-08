using FinTrack.Application.DTOs.Transactions;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class UpdateTransactionRequestValidator
        : AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionRequestValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty();

            RuleFor(x => x.CategoryId)
                .Must(categoryId => categoryId != Guid.Empty)
                .When(x => x.CategoryId.HasValue)
                .WithMessage("CategoryId must be a valid GUID.");

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.TransactionDirection)
                .IsInEnum();

            RuleFor(x => x.Merchant)
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.TransactionDate)
                .NotEmpty();
        }
    }
}