using Pharmacy.Application.Features.PharmacyInfo.Dtos;

namespace Pharmacy.Application.Features.PharmacyInfo.Validators
{
    public class PharmacyUpdateDtoValidator : AbstractValidator<PharmacyUpdateDto>
    {
        public PharmacyUpdateDtoValidator() 
        {
            RuleFor(p => p.StoreName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 120)
            .WithMessage("{PropertyName} must be between 3 to 120 characters");

            RuleFor(p => p.AddressLine1)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 120)
                .WithMessage("{PropertyName} must be between 3 to 120 characters");

            RuleFor(p => p.AddressLine2)
                .MaximumLength(120)
                .WithMessage("{PropertyName} can not be more than 120 characters");

            RuleFor(p => p.StoreLogo)
                .SetValidator(new PharmacyLogoValidator())
                .When(p => p.StoreLogo != null);
        }
    }
}
