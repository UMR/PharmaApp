using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Models;
using Pharmacy.Domain.Enums;
using Pharmacy.Domain;
using Pharmacy.Application.Features.Authentication.Validators;
using Pharmacy.Application.Exceptions;

namespace Pharmacy.Application.Features.Authentication.Services
{
    public class AuthenticationService: IAuthenticationService
    {

        #region Fields

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctro

        public AuthenticationService(IUserRepository userRepository, 
                                    IServiceProvider serviceProvider, 
                                    IRoleRepository roleRepository,
                                    IIdentityService identityService)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
            _roleRepository = roleRepository;
            _identityService = identityService;
        }

        #endregion

        #region Methods

        public async Task<TokenResponseDto> RegisterAsync(UserRegisterRequestDto request)
        {             
            var validator = new UserRegisterDtoValidator(_serviceProvider);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                throw new ValidationRequestException(validationResult.Errors);
            }

            Guid userId = Guid.NewGuid();
            var user = new Pharmacy.Domain.User();
            user.Id = userId;
            user.Pin = request.Pin;
            user.Status = (byte)UserStatusEnum.Pending;
            user.CreatedBy = userId;
            user.CreatedDate = DateTime.UtcNow;

            if (request.LoginId.Contains("@"))
            {
                user.Email = request.LoginId;
                user.EnrolledBy = "Email";
            }
            else
            {
                user.Mobile = request.LoginId;
                user.EnrolledBy = "Mobile";
            }

            var role = await _roleRepository.GetByNameAsync(RoleEnum.Pharmacist.ToString());

            if (role != null)
            {
                user.UserRoles = new UserRole[] { new UserRole() { UserId = userId, RoleId = role.Id } };
            }

            var createdUserId = await _userRepository.CreateAsync(user);

            if (createdUserId != Guid.Empty)
            {
                var response = await _identityService.GetToken(request.LoginId, request.Pin);
                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    return response;
                }
            }

            return new TokenResponseDto();        
        }

        public async Task<TokenResponseDto> LoginAsync(UserLoginRequestDto request)
        {
            var validator = new UserLoginDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                throw new ValidationRequestException(validationResult.Errors);
            }

            var response = await _identityService.GetToken(request.LoginId, request.Pin);

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                var user = await _userRepository.GetAsync(request.LoginId, request.Pin);

                if (user != null)
                {
                    return response;                
                }
            }

            return response;
        }

        #endregion
    }
}
