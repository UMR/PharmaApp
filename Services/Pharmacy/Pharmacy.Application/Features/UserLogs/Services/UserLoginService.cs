using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Application.Features.UserLogs.Services
{
    public class UserLoginService : IUserLoginService
    {
        #region Fields

        private readonly IUserLoginRepository _userLoginRepository;

        #endregion

        #region Ctor

        public UserLoginService(IUserLoginRepository userLoginRepository)
        {
            _userLoginRepository = userLoginRepository;
        }

        #endregion

        #region Methods

        public async Task CreateAsync(Guid userId)
        {
            var userLoginToCreate = new UserLogin
            {
                UserId = userId,
            };

            await _userLoginRepository.CreateAsync(userLoginToCreate);
        }

        #endregion
    }
}
