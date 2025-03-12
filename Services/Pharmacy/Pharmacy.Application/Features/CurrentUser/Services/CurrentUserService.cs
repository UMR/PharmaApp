using Microsoft.AspNetCore.Http;
using Pharmacy.Application.Features.User.Dtos;

namespace Pharmacy.Application.Features.CurrentUser.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctro

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Properties
        public Guid UserId
        {
            get
            {
                UserInfoDto currentUser = null;
                var currentUserClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("user");

                if (currentUserClaim != null && !string.IsNullOrEmpty(currentUserClaim.Value))
                {
                    currentUser = JsonConvert.DeserializeObject<UserInfoDto>(currentUserClaim.Value);
                }

                if (currentUser != null)
                {
                    return currentUser.Id;
                }

                return Guid.Empty;
            }
        }

        public string UserIpAddress
        {
            get
            {
                // var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.GetIpAddressString(); GetIpAddressString();
                //return ipAddress;
                return null;
            }
        }

        public UserInfoDto User
        {
            get
            {
                UserInfoDto currentUser = null;
                var currentUserClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("user");

                if (currentUserClaim != null && !string.IsNullOrEmpty(currentUserClaim.Value))
                {
                    currentUser = JsonConvert.DeserializeObject<UserInfoDto>(currentUserClaim.Value);
                }

                return currentUser;
            }
        }

        #endregion
    }
}
