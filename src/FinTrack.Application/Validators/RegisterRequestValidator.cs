using FinTrack.Application.DTOs.Authentication;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x=>x.FirstName).NotEmpty().MaximumLength(30);
            RuleFor(x=>x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency must be a valid 3-letter uppercase currency code.");

            RuleFor(x => x.TimeZone)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}