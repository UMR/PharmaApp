using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Constants;
using Pharmacy.Application.Contracts.Persistence;
using System.Text.RegularExpressions;

namespace Pharmacy.Application.Features.Authentication.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterRequestDto>
    {
        private readonly IUserRepository _userRepository;

        public UserRegisterDtoValidator(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();

            RuleFor(a => a.LoginId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Must(BeAValidEmailOrMobile)
                .WithMessage("{PropertyName} must be a valid email address or mobile number")
                .MustAsync(BeUniqueLoginId)
                .WithMessage("{PropertyName} already exists")
                .OverridePropertyName("LoginId");

            RuleFor(a => a.Pin)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .Matches(RegexConstant.SixDigitInteger)
                .WithMessage("{PropertyName} must be exactly 6 digits");           
        }

        private bool BeAValidEmailOrMobile(string loginId)
        {
            return IsValidEmail(loginId) || IsValidMobileNumber(loginId);
        }

        private async Task<bool> BeUniqueLoginId(string loginId, CancellationToken cancellationToken)
        {
            return !await _userRepository.IsExistAsync(loginId);
        }

        private bool IsValidMobileNumber(string loginId)
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

        private bool IsValidEmail(string loginId)
        {
            bool isEmail = Regex.IsMatch(loginId, RegexConstant.Email, RegexOptions.IgnoreCase);
            return isEmail;
        }
    }
}
