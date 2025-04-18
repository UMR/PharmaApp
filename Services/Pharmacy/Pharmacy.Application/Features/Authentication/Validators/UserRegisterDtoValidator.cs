using IdentityServer4.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Constants;
using System.Text.RegularExpressions;

namespace Pharmacy.Application.Features.Authentication.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterRequestDto>
    {
        private readonly IUserRepository _userRepository;

        public UserRegisterDtoValidator(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();

            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 30)
                .WithMessage("{PropertyName} must be between 3 to 30 characters");

            When(u => !string.IsNullOrEmpty(u.LastName), () =>
            {
                RuleFor(u => u.LastName)
                    .Length(3, 30)
                    .WithMessage("{PropertyName} must be between 3 to 30 characters");
            });

            When(u => !string.IsNullOrEmpty(u.Email), () =>
            {
                RuleFor(u => u.Email)
                .MustAsync(BeUniqueLoginId)
                .WithMessage("{PropertyName} already exists")
                .MustAsync(IsValidEmail)
                .WithMessage("{PropertyName} must be a valid email address");
            });
            
            RuleFor(u => u.Mobile)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MustAsync(IsValidMobileNumber)
                .WithMessage("{PropertyName} must be a valid mobile number")
                .MustAsync(BeUniqueLoginId)
                .WithMessage("{PropertyName} already exists");

            RuleFor(u => u.Pin)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Matches(RegexConstant.SixDigitInteger)
                .WithMessage("{PropertyName} must be exactly 6 digits");
        
        }

        private async Task<bool> BeUniqueLoginId(string loginId, CancellationToken cancellationToken)
        {
            return !await _userRepository.IsExistAsync(loginId);
        }

        private async Task<bool> IsValidMobileNumber(string loginId, CancellationToken cancellationToken)
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

        private async Task<bool> IsValidEmail(string loginId, CancellationToken cancellationToken)
        {
            bool isEmail = Regex.IsMatch(loginId, RegexConstant.Email, RegexOptions.IgnoreCase);
            return isEmail;
        }
    }
}
