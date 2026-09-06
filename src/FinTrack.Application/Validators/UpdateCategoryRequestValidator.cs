using FinTrack.Application.DTOs.Categories;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Description)
                .MaximumLength(50);
        }
    }
}