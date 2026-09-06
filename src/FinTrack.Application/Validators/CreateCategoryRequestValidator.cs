using FinTrack.Application.DTOs.Categories;
using FluentValidation;

namespace FinTrack.Application.Validators
{
    public class CreateCategoryRequestValidator:AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MaximumLength(30);
            RuleFor(x=>x.Description).MaximumLength(50);
        }
    }
}