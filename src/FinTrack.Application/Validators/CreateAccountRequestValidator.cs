using FinTrack.Application.DTOs.Accounts;
using FinTrack.Application.DTOs.Authentication;
using FinTrack.Domain.Enums;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class CreateAccountRequestValidator:AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MaximumLength(30);
            
            RuleFor(x=>x.AccountType).IsInEnum().WithMessage("Invalid account type.");

            RuleFor(x=>x.OpeningBalance) .GreaterThanOrEqualTo(0)
                .WithMessage("Opening balance cannot be negative.");

            RuleFor(x => x.Description).MaximumLength(100);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency must be a valid 3-letter uppercase currency code.");
        }
    }
}