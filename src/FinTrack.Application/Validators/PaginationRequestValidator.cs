using FinTrack.Application.DTOs.Common;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.Pagenumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.SortBy)
                .Must(x => 
                    x.Equals("TransactionDate", StringComparison.OrdinalIgnoreCase)||
                    x.Equals("Amount", StringComparison.OrdinalIgnoreCase)||
                    x.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase)||
                    x.Equals("Merchant", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortBy must be TransactionDate, Amount, CreatedAt or Merchant");

            RuleFor(x => x.SortOrder)
                .Must(x =>
                    x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortOrder must be asc or desc.");
        }
    }
}