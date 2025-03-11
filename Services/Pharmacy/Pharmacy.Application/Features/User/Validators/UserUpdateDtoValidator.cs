using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Constants;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.User.Dtos;
using System.Text.RegularExpressions;

namespace Pharmacy.Application.Features.User.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IUserRepository _userRepository;

        public UserUpdateDtoValidator(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();

            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(a => a.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters");

            RuleFor(a => a.LastName)
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters");

            RuleFor(a => a.Mobile)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(20)
                .WithMessage("{PropertyName} must not exceed 20 characters")
                .Must(BeAValidMobile)
                .WithMessage("{PropertyName} must be a valid mobile number");

            RuleFor(a => a.Email)
                .Matches(RegexConstant.Email)
                .When(a => !string.IsNullOrEmpty(a.Email))
                .WithMessage("{PropertyName} is not a valid email address")
                .MaximumLength(100)
                .WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(a => a.DateOfBirth)
                .Must(BeAPastDate)
                .WithMessage("The date must be in the past");
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

        private bool BeAPastDate(DateTime? date)
        {
            if (!date.HasValue)
                return true;

            return date.Value.Date <= DateTime.UtcNow.Date;
        }
    }
}
