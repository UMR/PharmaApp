using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;

namespace Pharmacy.Application.Features.PharmacyInfo.Validators
{
    public class PharmacyDtoValidator : AbstractValidator<PharmacyDto>
    {
        private readonly IPharmacyRepository _pharmacyRepository;

        public PharmacyDtoValidator(IServiceProvider serviceProvider)
        {
            _pharmacyRepository = serviceProvider.GetService<IPharmacyRepository>();

            RuleFor(p => p.StoreName.Trim())
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 120)
            .WithMessage("{PropertyName} must be between 3 to 120 characters");

            RuleFor(p => p.AddressLine1.Trim())
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 120)
                .WithMessage("{PropertyName} must be between 3 to 120 characters");

            When(p => !string.IsNullOrEmpty(p.AddressLine2.Trim()), () =>
            {
                RuleFor(p => p.AddressLine2.Trim())
                .MaximumLength(120)
                .WithMessage("{PropertyName} can not be more than 120 characters");
            });
            

            RuleFor(p => p.StoreLogo)
                .SetValidator(new PharmacyLogoValidator())
                .When(p => p.StoreLogo != null);

            When(p =>  p.Id != Guid.Empty, () =>
            {
                RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MustAsync(IsPharmacyAvailable).When(x => x.Id.HasValue)
                .WithMessage("Pharmacy with the specified {PropertyName} does not exist.");

            });
        }

        private async Task<bool> IsPharmacyAvailable(Guid? pharmacyId, CancellationToken cancellationToken)
        {
            return await _pharmacyRepository.IsPharmacyExistAsync(pharmacyId!.Value);
        }
    }
}
