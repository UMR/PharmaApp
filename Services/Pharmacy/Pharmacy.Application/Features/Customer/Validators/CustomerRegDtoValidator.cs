using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Constants;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.Customer.Dtos;
using System.Text.RegularExpressions;

namespace Pharmacy.Application.Features.Customer.Validators
{
    public class CustomerRegDtoValidator : AbstractValidator<CustomerRegDto>
    {
        private readonly IPharmacyRepository _pharmacyRepository;
        
        public CustomerRegDtoValidator(IServiceProvider serviceProvider) 
        {
            _pharmacyRepository = serviceProvider.GetService<IPharmacyRepository>();

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters")
                .MinimumLength(3)
                .WithMessage("{PropertyName} must be at least 3 characters");

            RuleFor(c => c.LastName)
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters")
                .MinimumLength(3)
                .WithMessage("{PropertyName} must be at least 3 characters");

            RuleFor(c => c.Mobile)
                            .NotEmpty()
                            .WithMessage("{PropertyName} is required")
                            .MaximumLength(20)
                            .WithMessage("{PropertyName} must not exceed 20 characters")
                            .Must(BeAValidMobile)
                            .WithMessage("{PropertyName} must be a valid mobile number");

            RuleFor(c => c.Email)
                .Matches(RegexConstant.Email)
                .When(c => !string.IsNullOrEmpty(c.Email))
                .WithMessage("{PropertyName} is not a valid email address")
                .MaximumLength(100)
                .WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(c => c.Age)
                .NotEmpty()
                .WithMessage("{PropertyName} can not be empty")
                .NotNull()
                .WithMessage("{PropertyName} can not be null")
                .LessThanOrEqualTo((short) 200)
                .WithMessage("{PropertyName} must be less then 200");

            RuleFor(c => c.Weight)
            .NotEmpty()
            .WithMessage("{PropertyName} can not be empty")
            .NotNull()
            .WithMessage("{PropertyName} can not be null")
            .GreaterThanOrEqualTo((float) 3.00)
            .WithMessage("{PropertyName} must be greater then 3KG");

            RuleFor(c => c.PharmacyId)
                .NotEmpty()
                .WithMessage("{PropertyName} can not be empty")
                .MustAsync(IsPharmacyAvailableAsync)
                .WithMessage("{PropertyName} can be valid");
        }

        private bool BeAValidMobile(string loginId)
        { 
            foreach (var pattern in RegexConstant.Mobile)
            {
                if (Regex.IsMatch(loginId, pattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> IsPharmacyAvailableAsync(Guid pharmacyId, CancellationToken cancellationToken)
        {
            var pharmacy = await _pharmacyRepository.GetPharmacyByUserIdAsync(pharmacyId);

            return (pharmacy == null);
        }
    }
}
