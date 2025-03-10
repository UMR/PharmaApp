namespace Pharmacy.Application.Features.Authentication.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginRequestDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(a => a.LoginId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(100)
                .WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(a => a.Pin)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters");            
        }
    }
}
