using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Exceptions;
using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Application.Features.User.Validators;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Features.User.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> GetUserAsync(string loginId, string password)
        {
            var userFromRepo = await _userRepository.GetAsync(loginId, password);
            var userToReturn = _mapper.Map<UserInfoDto>(userFromRepo);
            return userToReturn;
        }

        public async Task<bool> IsActiveAsync(Guid id)
        {
            var statusFromRepo = await _userRepository.IsActiveAsync(id);
            if (statusFromRepo != null)
            {
                var status = (UserStatusEnum)statusFromRepo;
                if (status == UserStatusEnum.Active || status == UserStatusEnum.Pending)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<UserInfoDto> GetByIdAsync(Guid id)
        {
            var userFromRepo = await _userRepository.GetByIdAsync(id);
            var userToReturn = _mapper.Map<UserInfoDto>(userFromRepo);       

            return userToReturn;
        }

        public async Task UpdateAsync(Guid id, UserUpdateDto request)
        {
            var validator = new UserUpdateDtoValidator(_serviceProvider);
            var validationResult = await validator.ValidateAsync(request);

            if (id != request.Id)
            {
                throw new BadRequestException("Id does not match");
            }

            if (validationResult.IsValid == false)
            {
                throw new ValidationRequestException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), id.ToString());
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                var isExistEmail = await _userRepository.IsExistEmailAsync(id, request.Email);
                if (isExistEmail)
                {
                    throw new BadRequestException("Email address already exist");
                }
            }

            var isExistMobile = await _userRepository.IsExistMobileAsync(id, request.Mobile);
            if (isExistMobile)
            {
                throw new BadRequestException("Mobile number already exist");
            }

            user.Id = id;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }
            user.Mobile = request.Mobile;
            user.Status = (byte)UserStatusEnum.Active;
            user.UpdatedBy = id;
            user.UpdatedDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
        }
    }
}
