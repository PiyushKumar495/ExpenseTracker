using FinTrack.Application.DTOs.Transactions;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class CreateTransferRequestValidator
        : AbstractValidator<CreateTransferRequest>
    {
        public CreateTransferRequestValidator()
        {
            RuleFor(x => x.FromAccountId)
                .NotEmpty();

            RuleFor(x => x.ToAccountId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(200);
        }
    }
}