using FinTrack.Application.DTOs.Accounts;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class UpdateAccountRequestValidator
        : AbstractValidator<UpdateAccountRequest>
    {
        public UpdateAccountRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage(
                    "Currency must be a valid 3-letter uppercase currency code.");

            RuleFor(x => x.Description)
                .MaximumLength(100);
        }
    }
}