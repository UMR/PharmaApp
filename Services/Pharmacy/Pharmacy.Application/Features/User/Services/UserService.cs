using AutoMapper;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Features.User.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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
    }
}
